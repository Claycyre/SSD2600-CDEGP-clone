using System.Collections.Generic;
using System.Linq;

namespace SSD2600_CDEGP.Models;

public class CartItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    public decimal LineTotal => Price * Quantity;
}

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.LineTotal);

    public int ItemCount => Items.Sum(i => i.Quantity);
}
