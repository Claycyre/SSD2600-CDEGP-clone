using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Data;

namespace SSD2600_CDEGP.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
