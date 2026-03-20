using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Services;

namespace SSD2600_CDEGP.Controllers;

public class CartController(CartService cartService, ApplicationDbContext db) : Controller
{
    public IActionResult Index()
    {
        return View(cartService.GetCart());
    }

    [HttpPost]
    public async Task<IActionResult> Add(string id, int quantity = 1)
    {
        if (!int.TryParse(id, out int sku))
            return BadRequest();

        var product = await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.PkSKU == sku);
        if (product == null)
            return NotFound();

        cartService.AddItem(id, product.Name, (decimal)product.UnitPrice, quantity, null);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(string id)
    {
        cartService.RemoveItem(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(string id, int quantity)
    {
        cartService.UpdateQuantity(id, quantity);
        return RedirectToAction("Index");
    }

    public IActionResult Checkout()
    {
        return RedirectToAction("Index", "Checkout");
    }
}
