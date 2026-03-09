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
        [FromQuery(Name = "ap")] List<string>? applications,
        [FromQuery(Name = "ph")] List<string>? phases,
        string? sortBy
    )
    {
        var model = new FilterModel(elementService)
        {
            SelectedPhases = phases ?? [],
            SortBy = sortBy,
            SelectedApplications = applications ?? [],
        };

        // When product-type filters are active, load matching atomic numbers from the DB
        if (model.SelectedApplications.Count > 0)
        {
            var atomicNumbers = await db
                .Products.AsNoTracking()
                .Where(p =>
                    model.SelectedApplications.Contains(p.ProductType) && p.AtomicNumber != null
                )
                .Select(p => p.AtomicNumber!.Value)
                .Distinct()
                .ToListAsync();
            model.AvailableAtomicNumbers = [.. atomicNumbers];
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
