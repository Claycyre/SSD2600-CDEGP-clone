using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Controllers;

public class HomeController(ILogger<HomeController> logger, ElementService elementService)
    : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [HttpGet]
    public IActionResult Index(List<string>? phases, string? sortBy)
    {
        var model = new IndexModel(elementService) { Phases = phases ?? [], SortBy = sortBy };
        model.ApplyFilters();

        return View(model);
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
