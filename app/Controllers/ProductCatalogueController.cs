using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Controllers;

public class ProductCatalogueController(
    ILogger<ProductCatalogueController> logger,
    ApplicationDbContext db,
    UserManager<ApplicationUser> userManager,
    ElementService elementService
) : Controller
{
    private readonly ILogger<ProductCatalogueController> _logger = logger;
    private readonly ApplicationDbContext _db = db;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ElementService _elementService = elementService;

    public async Task<IActionResult> Index(
        [FromQuery(Name = "an")] int? element,
        [FromQuery(Name = "q")] string? search,
        [FromQuery(Name = "ap")] List<string>? applications,
        [FromQuery(Name = "ph")] List<string>? phases
    )
    {
        IQueryable<Product> productsQuery = _db.Products.AsNoTracking().Include(p => p.Supplier);

        if (element.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.AtomicNumber == element.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(search)
                || (p.ShortName != null && p.ShortName.Contains(search))
                || (p.Description != null && p.Description.Contains(search))
            );
        }

        if (applications?.Count > 0)
            productsQuery = productsQuery.Where(p => applications.Contains(p.ProductType));

        if (phases?.Count > 0)
            productsQuery = productsQuery.Where(p => phases.Contains(p.StateOfMatter));

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

        var vm = new FilterCatalogueModel(_elementService)
        {
            Products = products,
            Query = search,
            RecentPurchases = recentPurchases,
            SelectedApplications = applications ?? [],
            SelectedPhases = phases ?? [],
            PreferredCurrencyCode = preferredCurrency,
        };

        return View(vm);
    }

    // GET: /ProductCatalogue/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        var product = _db
            .Products.AsNoTracking()
            .Include(p => p.Supplier)
            .FirstOrDefault(p => p.PkSKU == id);

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
            Id = product.PkSKU,
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
            SupplierName = product.Supplier?.Name,
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
