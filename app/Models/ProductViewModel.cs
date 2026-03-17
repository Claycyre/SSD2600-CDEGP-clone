namespace SSD2600_CDEGP.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string StateOfMatter { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string? ProductSubtype { get; set; }
    public string? HalfLife { get; set; }
    public string? Purity { get; set; }
    public string? SpecificActivity { get; set; }
    public string CurrencyCode { get; set; } = "CAD";
    public string? SupplierName { get; set; }
}
