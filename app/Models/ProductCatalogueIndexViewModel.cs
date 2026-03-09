using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Models;

public class RecentPurchaseViewModel
{
    public Product Product { get; set; } = null!;
    public double PriceAtPurchase { get; set; }
    public string CurrencyCode { get; set; } = "CAD";
    public DateTime PurchasedAt { get; set; }
}

public class FilterCatalogueModel(ElementService elementService) : FilterModel(elementService)
{
    public List<Product> Products { get; set; } = [];

    public string? Query { get; set; }

    public List<RecentPurchaseViewModel> RecentPurchases { get; set; } = [];

    /// <summary>ISO 4217 currency code used to display prices (from the logged-in user or defaults to CAD).</summary>
    public string PreferredCurrencyCode { get; set; } = "CAD";
}
