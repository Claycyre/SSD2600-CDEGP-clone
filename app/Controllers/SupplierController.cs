using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;

namespace SSD2600_CDEGP.Controllers;

[Authorize]
public class SupplierController : Controller
{
    private readonly SupplierRepository _supplierRepository;
    private readonly ProductRepository _productRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public SupplierController(
        SupplierRepository supplierRepository,
        ProductRepository productRepository,
        UserManager<ApplicationUser> userManager
    )
    {
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
        _userManager = userManager;
    }

    // GET: Supplier/Index
    public async Task<IActionResult> Index()
    {
        // Check if user is associated with a supplier
        var user = await _userManager.GetUserAsync(User);

        if (user?.FkSupplierId == null)
            return Unauthorized("You are not associated with a supplier.");

        var products = await _productRepository.GetBySupplierIdAsync(user.FkSupplierId.Value);
        // Get supplier info for display
        var supplier = await _supplierRepository.GetByIdAsync(user.FkSupplierId.Value);

        ViewData["SupplierId"] = user.FkSupplierId.Value;
        ViewData["SupplierName"] = supplier?.Name ?? "Unknown Supplier";

        return View(products);
    }

    // GET: Supplier/EditProduct/5
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        // Get current user
        var user = await _userManager.GetUserAsync(User);

        // Verify user can edit this product
        if (user?.FkSupplierId != product.FkSupplierId)
            return Unauthorized("You can only edit products for your supplier.");

        return View(product);
    }

    // POST: Supplier/EditProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(
        int id,
        [Bind("PkSKU,Name,Description,StockQuantity,FkSupplierId")] Product product
    )
    {
        if (id != product.PkSKU)
            return NotFound();

        // Get current user
        var user = await _userManager.GetUserAsync(User);

        // Verify user can edit this product
        if (user?.FkSupplierId != product.FkSupplierId)
            return Unauthorized("You can only edit products for your supplier.");

        if (ModelState.IsValid)
        {
            await _productRepository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // POST: Supplier/RemoveProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        // Get current user
        var user = await _userManager.GetUserAsync(User);

        // Verify user can remove this product
        if (user?.FkSupplierId != product.FkSupplierId)
            return Unauthorized("You can only manage products for your supplier.");

        // Soft delete by removing supplier association
        product.FkSupplierId = 0;
        await _productRepository.UpdateAsync(product);

        return RedirectToAction(nameof(Index));
    }
}
