using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            {
                return View();
            }
        }
    }
}
