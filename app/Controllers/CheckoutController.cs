using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Controllers;

[Authorize]
public class CheckoutController(
    CartService cartService,
    PayPalService payPalService,
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    IOptions<PayPalSettings> payPalOptions
) : Controller
{
    private readonly PayPalSettings _payPalSettings = payPalOptions.Value;

    // GET /Checkout
    public async Task<IActionResult> Index()
    {
        var cart = cartService.GetCart();
        if (!cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        var user = await userManager.GetUserAsync(User);
        ContactDetail? contact = null;

        if (user?.FkContactId != null)
            contact = await db.ContactDetail.FindAsync(user.FkContactId);

        var vm = new CheckoutViewModel
        {
            Cart = cart,
            ContactDetail = contact,
            CurrencyCode = user?.PreferredCurrencyCode ?? "CAD",
            PayPalClientId = _payPalSettings.ClientId,
        };

        return View(vm);
    }

    // POST /Checkout/CreatePayPalOrder — called by PayPal JS SDK
    [HttpPost]
    public async Task<IActionResult> CreatePayPalOrder()
    {
        var cart = cartService.GetCart();
        if (!cart.Items.Any())
            return BadRequest(new { error = "Cart is empty" });

        var user = await userManager.GetUserAsync(User);
        var currency = user?.PreferredCurrencyCode ?? "CAD";

        var paypalOrderId = await payPalService.CreateOrderAsync(cart, currency);
        return Json(new { id = paypalOrderId });
    }

    // POST /Checkout/CapturePayPalOrder — called by PayPal JS SDK after user approves
    [HttpPost]
    public async Task<IActionResult> CapturePayPalOrder(
        [FromBody] CapturePayPalOrderRequest request
    )
    {
        var cart = cartService.GetCart();
        if (!cart.Items.Any())
            return BadRequest(new { error = "Cart is empty" });

        var user = await userManager.GetUserAsync(User);
        var currency = user?.PreferredCurrencyCode ?? "CAD";

        var result = await payPalService.CaptureOrderAsync(request.OrderId);
        if (!result.Success)
            return Json(new { success = false, error = result.ErrorMessage });

        // Persist Transaction
        var transaction = new Transaction
        {
            GatewayTransactionId = result.CaptureId!,
            GatewayName = "PayPal",
            Subtotal = (double)cart.Total,
            Total = (double)cart.Total,
            CurrencyCode = currency,
        };
        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();

        // Persist Order
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            OrderDate = DateTime.UtcNow,
            Status = "Processing",
            FkUserId = user!.Id,
            FkTransactionId = transaction.PkTransactionId,
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync();

        // Persist OrderLineItems
        foreach (var item in cart.Items)
        {
            if (!int.TryParse(item.Id, out int sku))
                continue;

            db.OrderLineItems.Add(
                new OrderLineItem
                {
                    FkOrderId = order.PkOrderId,
                    FkProductSKU = sku,
                    Quantity = item.Quantity,
                    UnitPrice = (double)item.Price,
                }
            );
        }
        await db.SaveChangesAsync();

        cartService.ClearCart();

        return Json(new { success = true, orderId = order.PkOrderId });
    }

    // GET /Checkout/Success?orderId=
    public async Task<IActionResult> Success(int orderId)
    {
        var userId = userManager.GetUserId(User);

        var order = await db
            .Orders.Include(o => o.OrderLineItems)
                .ThenInclude(li => li.Product)
            .Include(o => o.Transaction)
            .FirstOrDefaultAsync(o => o.PkOrderId == orderId && o.FkUserId == userId);

        if (order == null)
            return NotFound();

        return View(order);
    }

    // GET /Checkout/Cancel
    public IActionResult Cancel() => View();

    private static string GenerateOrderNumber()
    {
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = Random.Shared.Next(10000, 99999);
        return $"ORD-{datePart}-{randomPart}";
    }
}

public class CapturePayPalOrderRequest
{
    public string OrderId { get; set; } = string.Empty;
}
