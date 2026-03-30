namespace SSD2600_CDEGP.Models;

public class PayPalSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Mode { get; set; } = "sandbox";

    public string BaseUrl =>
        Mode == "live" ? "https://api-m.paypal.com" : "https://api-m.sandbox.paypal.com";
}
