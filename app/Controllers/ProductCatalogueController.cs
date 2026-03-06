using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

public class ProductCatalogueController(
    ILogger<ProductCatalogueController> logger,
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager
) : Controller
{
    private readonly ILogger<ProductCatalogueController> _logger = logger;
    private readonly ApplicationDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IActionResult> Index(
        string? search,
        List<string>? types,
        List<string>? states
    )
    {
        var productsQuery = _db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(search)
                || (p.ShortName != null && p.ShortName.Contains(search))
                || (p.Description != null && p.Description.Contains(search))
            );
        }

        if (types?.Count > 0)
            productsQuery = productsQuery.Where(p => types.Contains(p.ProductType));

        if (states?.Count > 0)
            productsQuery = productsQuery.Where(p => states.Contains(p.StateOfMatter));

        var products = await productsQuery.ToListAsync();

        // Determine user's preferred currency (defaults to CAD)
        string preferredCurrency = "CAD";
        List<RecentPurchaseViewModel> recentPurchases = [];

        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = _userManager.GetUserId(User);
            var appUser = await _userManager.FindByIdAsync(userId!);
            if (appUser != null)
                preferredCurrency = appUser.PreferredCurrencyCode;

            //recentPurchases = await _db
            //    .UserPurchases.AsNoTracking()
            //    .Where(up => up.FkUserId == userId && up.Product != null)
            //    .OrderByDescending(up => up.PurchasedAt)
            //    .Include(up => up.Product)
            //    .Take(3)
            //    .Select(up => new RecentPurchaseViewModel
            //    {
            //        Product = up.Product!,
            //        PriceAtPurchase = up.PriceAtPurchase,
            //        CurrencyCode = up.CurrencyCode,
            //        PurchasedAt = up.PurchasedAt,
            //    })
            //    .ToListAsync();
        }

        var vm = new ProductCatalogueIndexViewModel
        {
            Products = products,
            SearchQuery = search,
            RecentPurchases = recentPurchases,
            SelectedTypes = types ?? [],
            SelectedStates = states ?? [],
            PreferredCurrencyCode = preferredCurrency,
        };

        return View(vm);
    }

    // GET: /ProductCatalogue/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        var product = _db.Products.AsNoTracking().FirstOrDefault(p => p.PkSKU == id);

        if (product == null)
        {
            return NotFound();
        }

        string currencyCode = "CAD";
        if (User.Identity?.IsAuthenticated == true)
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser != null)
                currencyCode = appUser.PreferredCurrencyCode;
        }

        var viewModel = new ProductViewModel
        {
            Id = product.PkSKU.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = (decimal)product.UnitPrice,
            ImageUrl = null,
            StateOfMatter = product.StateOfMatter,
            ProductType = product.ProductType,
            ProductSubtype = product.ProductSubtype,
            HalfLife = product.HalfLife,
            Purity = product.Purity,
            SpecificActivity = product.SpecificActivity,
            CurrencyCode = currencyCode,
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
