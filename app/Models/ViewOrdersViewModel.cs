namespace SSD2600_CDEGP.Models;

/// <summary>
/// A single product within an order.
/// </summary>
public class OrderItemViewModel
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductImage { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Specifications { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string ManufacturerDescription { get; set; } = string.Empty;
}

/// <summary>
/// A group of items ordered together on the same date.
/// </summary>
public class OrderGroupViewModel
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusTimeframe { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
}

public class ViewOrdersViewModel
{
    /// <summary>Order groups sorted by date descending (latest first).</summary>
    public List<OrderGroupViewModel> OrderGroups { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string UserAvatarInitials { get; set; } = string.Empty;
    public string? NotificationMessage { get; set; }
}
