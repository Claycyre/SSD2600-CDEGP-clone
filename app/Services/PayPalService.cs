using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Services;

public class PayPalCaptureResult
{
    public bool Success { get; set; }
    public string? CaptureId { get; set; }
    public string? PayerEmail { get; set; }
    public string? ErrorMessage { get; set; }
}

public class PayPalService(
    IHttpClientFactory httpClientFactory,
    IOptions<PayPalSettings> options,
    IMemoryCache cache
)
{
    private readonly PayPalSettings _settings = options.Value;
    private const string TokenCacheKey = "paypal_access_token";

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private HttpClient CreateClient() => httpClientFactory.CreateClient("paypal");

    // ── Auth ─────────────────────────────────────────────────────────────────

    private async Task<string> GetAccessTokenAsync()
    {
        if (cache.TryGetValue(TokenCacheKey, out string? cached) && cached != null)
            return cached;

        var client = CreateClient();
        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}")
        );

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_settings.BaseUrl}/v1/oauth2/token"
        )
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Basic", credentials) },
            Content = new StringContent(
                "grant_type=client_credentials",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            ),
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var token = doc.RootElement.GetProperty("access_token").GetString()!;
        var expiresIn = doc.RootElement.GetProperty("expires_in").GetInt32();

        cache.Set(TokenCacheKey, token, TimeSpan.FromSeconds(expiresIn - 60));
        return token;
    }

    // ── Create Order ─────────────────────────────────────────────────────────

    public async Task<string> CreateOrderAsync(CartViewModel cart, string currencyCode)
    {
        var token = await GetAccessTokenAsync();
        var client = CreateClient();

        var items = cart
            .Items.Select(i => new
            {
                name = i.Name.Length > 127 ? i.Name[..127] : i.Name,
                quantity = i.Quantity.ToString(),
                unit_amount = new { currency_code = currencyCode, value = i.Price.ToString("F2") },
            })
            .ToList();

        var body = new
        {
            intent = "CAPTURE",
            purchase_units = new[]
            {
                new
                {
                    amount = new
                    {
                        currency_code = currencyCode,
                        value = cart.Total.ToString("F2"),
                        breakdown = new
                        {
                            item_total = new
                            {
                                currency_code = currencyCode,
                                value = cart.Total.ToString("F2"),
                            },
                        },
                    },
                    items,
                },
            },
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_settings.BaseUrl}/v2/checkout/orders"
        )
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            Content = new StringContent(
                JsonSerializer.Serialize(body, JsonOpts),
                Encoding.UTF8,
                "application/json"
            ),
        };
        request.Headers.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("id").GetString()!;
    }

    // ── Capture Order ─────────────────────────────────────────────────────────

    public async Task<PayPalCaptureResult> CaptureOrderAsync(string paypalOrderId)
    {
        var token = await GetAccessTokenAsync();
        var client = CreateClient();

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{_settings.BaseUrl}/v2/checkout/orders/{paypalOrderId}/capture"
        )
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            Content = new StringContent("{}", Encoding.UTF8, "application/json"),
        };

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return new PayPalCaptureResult { Success = false, ErrorMessage = error };
        }

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var status = doc.RootElement.GetProperty("status").GetString();
        if (status != "COMPLETED")
            return new PayPalCaptureResult
            {
                Success = false,
                ErrorMessage = $"Unexpected capture status: {status}",
            };

        var capture = doc
            .RootElement.GetProperty("purchase_units")[0]
            .GetProperty("payments")
            .GetProperty("captures")[0];

        var captureId = capture.GetProperty("id").GetString()!;

        string? payerEmail = null;
        if (
            doc.RootElement.TryGetProperty("payer", out var payer)
            && payer.TryGetProperty("email_address", out var email)
        )
            payerEmail = email.GetString();

        return new PayPalCaptureResult
        {
            Success = true,
            CaptureId = captureId,
            PayerEmail = payerEmail,
        };
    }
}
