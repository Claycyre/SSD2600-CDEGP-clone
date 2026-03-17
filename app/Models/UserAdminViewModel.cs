namespace SSD2600_CDEGP.Models;

public class UserAdminViewModel
{
    public List<ApplicationUser> Users { get; set; } = [];
    public string? SearchQuery { get; set; }
    public int PendingVerificationCount { get; set; }
    public int PendingProductCount { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}
