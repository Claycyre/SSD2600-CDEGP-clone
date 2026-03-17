namespace SSD2600_CDEGP.Models;

public class UserDetailViewModel
{
    public ApplicationUser User { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
    public List<Product> Products { get; set; } = [];
    public List<AdminMessage> Messages { get; set; } = [];
    public string CurrentAdminId { get; set; } = string.Empty;
}
