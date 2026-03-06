using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    ElementService elementService,
    ApplicationDbContext db
) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> Index(
        List<string>? phases,
        string? sortBy,
        List<string>? types
    )
    {
        var model = new IndexModel(elementService)
        {
            Phases = phases ?? [],
            SortBy = sortBy,
            SelectedTypes = types ?? [],
        };

        // When product-type filters are active, load matching atomic numbers from the DB
        if (model.SelectedTypes.Count > 0)
        {
            var atomicNumbers = await db
                .Products.AsNoTracking()
                .Where(p => model.SelectedTypes.Contains(p.ProductType) && p.AtomicNumber != null)
                .Select(p => p.AtomicNumber!.Value)
                .Distinct()
                .ToListAsync();
            model.ProductAtomicNumbers = [.. atomicNumbers];
        }

        model.ApplyFilters();

        return View(model);
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
