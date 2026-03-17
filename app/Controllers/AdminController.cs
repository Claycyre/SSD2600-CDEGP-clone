using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public AdminController(
        ILogger<AdminController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db
    )
    {
        _logger = logger;
        _userManager = userManager;
        _db = db;
    }

    // GET: /Admin
    public async Task<IActionResult> Index(string? search)
    {
        var query = _userManager.Users.Include(u => u.Supplier).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var upper = search.Trim().ToUpperInvariant();
            query = query.Where(u =>
                (u.NormalizedEmail != null && u.NormalizedEmail.Contains(upper))
                || (u.NormalizedUserName != null && u.NormalizedUserName.Contains(upper))
                || (u.Id != null && u.Id.Contains(search.Trim()))
                || (u.PhoneNumber != null && u.PhoneNumber.Contains(search.Trim()))
            );
        }

        var users = await query.OrderBy(u => u.UserName).ToListAsync();

        var pendingCount = await _userManager.Users.CountAsync(u =>
            u.VerificationSubmitted && !u.VerificationApproved
        );

        var pendingProductCount = await _db.Products.CountAsync(p => !p.IsAdminVerified);

        var model = new UserAdminViewModel
        {
            Users = users,
            SearchQuery = search,
            PendingVerificationCount = pendingCount,
            PendingProductCount = pendingProductCount,
            CurrentUserId = _userManager.GetUserId(HttpContext.User) ?? string.Empty,
        };

        return View(model);
    }

    // GET: /Admin/User/{userId}
    public new async Task<IActionResult> User(string userId)
    {
        var user = await _userManager
            .Users.Include(u => u.Supplier)
            .Include(u => u.ContactDetail)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound();

        List<Product> products = [];
        if (user.FkSupplierId.HasValue)
        {
            products = await _db
                .Products.AsNoTracking()
                .Where(p => p.FkSupplierId == user.FkSupplierId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        var messages = await _db
            .AdminMessages.AsNoTracking()
            .Include(m => m.Sender)
            .Where(m => m.FkRecipientId == userId)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();

        var roles = await _userManager.GetRolesAsync(user);

        var model = new UserDetailViewModel
        {
            User = user,
            Roles = roles,
            Products = products,
            Messages = messages,
            CurrentAdminId = _userManager.GetUserId(HttpContext.User) ?? string.Empty,
        };

        return View(model);
    }

    // POST: /Admin/ApproveVerification
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveVerification(string userId, string? returnUrl = null)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.VerificationApproved = true;
            await _userManager.UpdateAsync(user);
        }
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // POST: /Admin/BanUser
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> BanUser(string userId, string? returnUrl = null)
    {
        if (userId == _userManager.GetUserId(HttpContext.User))
            return Forbid();

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && user.UserRole != "SiteAdmin")
        {
            user.UserBanned = true;
            user.UserSuspended = false;
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);

            // Remove all products belonging to a banned supplier
            if (user.UserRole == "Supplier" && user.FkSupplierId.HasValue)
            {
                await _db
                    .Products.Where(p => p.FkSupplierId == user.FkSupplierId.Value)
                    .ExecuteDeleteAsync();
            }
        }
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // POST: /Admin/UnbanUser
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UnbanUser(string userId, string? returnUrl = null)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.UserBanned = false;
            user.UserSuspended = false;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            await _userManager.UpdateAsync(user);
        }
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // POST: /Admin/PutOnHold
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PutOnHold(string userId, string? returnUrl = null)
    {
        if (userId == _userManager.GetUserId(HttpContext.User))
            return Forbid();

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !user.UserBanned && user.UserRole != "SiteAdmin")
        {
            user.UserSuspended = true;
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
        }
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // POST: /Admin/LiftHold
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LiftHold(string userId, string? returnUrl = null)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !user.UserBanned)
        {
            user.UserSuspended = false;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            await _userManager.UpdateAsync(user);
        }
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // GET: /Admin/MessageUser/{userId}
    public async Task<IActionResult> MessageUser(string userId)
    {
        var recipient = await _userManager.FindByIdAsync(userId);
        if (recipient == null)
            return NotFound();

        return View(
            new ComposeMessageViewModel
            {
                RecipientId = userId,
                RecipientName = recipient.UserName ?? recipient.Email ?? userId,
            }
        );
    }

    // POST: /Admin/MessageUser
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MessageUser(
        ComposeMessageViewModel model,
        string? returnUrl = null
    )
    {
        if (!ModelState.IsValid)
        {
            var recipient = await _userManager.FindByIdAsync(model.RecipientId);
            model.RecipientName = recipient?.UserName ?? recipient?.Email ?? model.RecipientId;
            return View(model);
        }

        _db.AdminMessages.Add(
            new AdminMessage
            {
                FkSenderId = _userManager.GetUserId(HttpContext.User)!,
                FkRecipientId = model.RecipientId,
                Subject = model.Subject,
                Body = model.Body,
                SentAt = DateTime.UtcNow,
            }
        );
        await _db.SaveChangesAsync();

        TempData["Success"] = "Message sent successfully.";
        return SafeRedirect(returnUrl, nameof(Index));
    }

    // GET: /Admin/Products
    public async Task<IActionResult> Products(string? sortBy)
    {
        var query = _db
            .Products.AsNoTracking()
            .Include(p => p.Supplier)
            .OrderBy(p => p.IsAdminVerified);

        IOrderedQueryable<Product> sorted = sortBy switch
        {
            "name" => query.ThenBy(p => p.Name),
            "supplier" => query.ThenBy(p => p.Supplier!.Name),
            "type" => query.ThenBy(p => p.ProductType),
            _ => query.ThenBy(p => p.Name),
        };

        var products = await sorted.ToListAsync();

        ViewBag.SortBy = sortBy;
        return View(products);
    }

    // POST: /Admin/ApproveProduct
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveProduct(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            product.IsAdminVerified = true;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Products));
    }

    // POST: /Admin/RejectProduct
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectProduct(int productId)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Products));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    private IActionResult SafeRedirect(string? returnUrl, string fallbackAction)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);
        return RedirectToAction(fallbackAction);
    }
}
