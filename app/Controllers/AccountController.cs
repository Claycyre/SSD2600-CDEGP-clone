using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers;

[Area("Account")]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    public IActionResult Register()
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
