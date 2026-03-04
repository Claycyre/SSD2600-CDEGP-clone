namespace SSD2600_CDEGP.Models;

public class RecentPurchaseViewModel
{
    public Product Product { get; set; } = null!;
    public double PriceAtPurchase { get; set; }
    public string CurrencyCode { get; set; } = "CAD";
    public DateTime PurchasedAt { get; set; }
}

public class ProductCatalogueIndexViewModel
{
    public List<Product> Products { get; set; } = [];
    public string? SearchQuery { get; set; }
    public List<RecentPurchaseViewModel> RecentPurchases { get; set; } = [];

    /// <summary>Product types currently checked in the filter (e.g. Medical, Industrial, Research).</summary>
    public List<string> SelectedTypes { get; set; } = [];

    /// <summary>States of matter currently checked in the filter (e.g. Solid, Liquid, Gas).</summary>
    public List<string> SelectedStates { get; set; } = [];

    /// <summary>ISO 4217 currency code used to display prices (from the logged-in user or defaults to CAD).</summary>
    public string PreferredCurrencyCode { get; set; } = "CAD";
}
