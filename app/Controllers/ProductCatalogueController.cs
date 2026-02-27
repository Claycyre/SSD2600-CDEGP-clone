using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

public class ProductCatalogueController : Controller
{
    private readonly ILogger<ProductCatalogueController> _logger;

    public ProductCatalogueController(ILogger<ProductCatalogueController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    // GET: /ProductCatalogue/Details/{id}
    public IActionResult Details(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        // In a real app, you'd load product data from a database.
        // Here we return a sample product based on the id for demonstration.
        var product = new ProductViewModel
        {
            Id = id,
            Name = id == "1" ? "Helium 3 Canister" : "Lunar Soil Sample",
            Description =
                "Detailed description and specifications for the product. To be filled out later.",
            Price = id == "1" ? 1999.99M : 499.50M,
            ImageUrl = null,
        };

        return View(product);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
