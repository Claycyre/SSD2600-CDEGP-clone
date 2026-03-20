namespace SSD2600_CDEGP.Models;

public class CheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new();
    public ContactDetail? ContactDetail { get; set; }
    public string CurrencyCode { get; set; } = "CAD";
    public string PayPalClientId { get; set; } = string.Empty;
}
