using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;

namespace SSD2600_CDEGP.Controllers;

[Authorize]
public class OrdersController(
    OrderRepository orderRepository,
    UserManager<ApplicationUser> userManager
) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var orders = await orderRepository.GetOrdersByUserIdAsync(user.Id);

        var firstName = user.ContactDetail?.NameFirst ?? user.UserName ?? "User";
        var lastName = user.ContactDetail?.NameLast ?? string.Empty;
        var fullName = $"{firstName} {lastName}".Trim();
        var initials = GetInitials(firstName, lastName);

        var orderGroups = orders.Select(o => new OrderGroupViewModel
        {
            OrderId = o.PkOrderId,
            OrderNumber = o.OrderNumber,
            OrderDate = o.OrderDate,
            Status = o.Status,
            StatusTimeframe = GetStatusTimeframe(o.Status),
            TotalPrice = o.OrderLineItems.Count > 0
                ? (decimal)o.OrderLineItems.Sum(li => li.Quantity * li.UnitPrice)
                : (decimal)(o.Transaction?.Total ?? 0),
            Items = o.OrderLineItems.Select(li => new OrderItemViewModel
            {
                Id = li.FkProductSKU,
                ProductName = li.Product?.Name ?? "Unknown Product",
                ProductImage = string.Empty,
                Description = li.Product?.Description ?? string.Empty,
                Price = (decimal)li.UnitPrice,
                Specifications = BuildSpecifications(li.Product),
                Manufacturer = li.Product?.Supplier?.Name ?? string.Empty,
                ManufacturerDescription = li.Product?.Supplier?.ShortName ?? string.Empty
            }).ToList()
        }).ToList();

        var vm = new ViewOrdersViewModel
        {
            OrderGroups = orderGroups,
            UserName = fullName,
            UserRole = "Customer",
            UserAvatarInitials = initials,
            NotificationMessage = TempData["Notification"] as string
        };

        return View(vm);
    }

    [HttpPost]
    public IActionResult OrderAgain(int orderId)
    {
        TempData["Notification"] = "Order has been placed again successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult ReturnItem(int orderId)
    {
        TempData["Notification"] = "Return request has been submitted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult LeaveFeedback(int orderId, string feedback)
    {
        TempData["Notification"] = "Thank you for your feedback!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult DismissNotification()
    {
        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string GetInitials(string firstName, string lastName)
    {
        var first = firstName.Length > 0 ? firstName[0].ToString().ToUpper() : string.Empty;
        var last = lastName.Length > 0 ? lastName[0].ToString().ToUpper() : string.Empty;
        return $"{first}{last}";
    }

    private static string GetStatusTimeframe(string status) => status switch
    {
        "Processing" => "5–7 business days",
        "Shipped" => "2–3 business days",
        "Delivered" => "Completed",
        "Cancelled" => "Cancelled",
        _ => "In progress"
    };

    private static string BuildSpecifications(Product? product)
    {
        if (product == null) return string.Empty;

        List<string> parts = [];
        if (product.AtomicNumber.HasValue) parts.Add($"Atomic Number: {product.AtomicNumber}");
        if (!string.IsNullOrEmpty(product.StateOfMatter)) parts.Add($"State: {product.StateOfMatter}");
        if (!string.IsNullOrEmpty(product.ProductType)) parts.Add($"Type: {product.ProductType}");
        if (!string.IsNullOrEmpty(product.HalfLife)) parts.Add($"Half-Life: {product.HalfLife}");
        if (!string.IsNullOrEmpty(product.Purity)) parts.Add($"Purity: {product.Purity}");
        if (!string.IsNullOrEmpty(product.SpecificActivity)) parts.Add($"Specific Activity: {product.SpecificActivity}");
        return string.Join("\n", parts);
    }
}
