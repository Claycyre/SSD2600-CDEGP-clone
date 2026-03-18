using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;
using SSD2600_CDEGP.Repositories;

namespace SSD2600_CDEGP.Controllers;

[Authorize(Roles = "Supplier")]
public class SupplierController(
    ApplicationDbContext _db,
    SupplierRepository supplierRepository,
    ProductRepository productRepository,
    UserManager<ApplicationUser> userManager
) : Controller
{
    private async Task<int?> GetCurrentSupplierIdAsync()
    {
        var user = await userManager.GetUserAsync(User);
        return user?.FkSupplierId;
    }

    // GET: Supplier/Index
    public async Task<IActionResult> Index()
    {
        // Check if user is associated with a supplier
        var user = await userManager.GetUserAsync(User);
        var products = await productRepository.GetBySupplierIdAsync(user.FkSupplierId.Value);
        // Get supplier info for display
        var supplier = await supplierRepository.GetByIdAsync(user.FkSupplierId.Value);

        ViewData["SupplierId"] = user.FkSupplierId.Value;
        ViewData["SupplierName"] = supplier?.Name ?? "Unknown Supplier";
        ViewBag.CurrencyCode = products.FirstOrDefault()?.Supplier?.CurrencyCode ?? "CAD";

        return View(products);
    }

    // GET: /Supplier/CreateProduct
    public IActionResult CreateProduct() => View(new ProductAddOrEditViewModel());

    // POST: /Supplier/CreateProduct
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(ProductAddOrEditViewModel vm)
    {
        var supplierId = await GetCurrentSupplierIdAsync();
        if (supplierId == null)
            return Forbid();

        if (!ModelState.IsValid)
            return View(vm);

        var product = VMToModel(vm, supplierId);

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Product submitted and is awaiting admin verification.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Supplier/EditProduct/5
    public async Task<IActionResult> EditProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        // Get current user
        var user = await userManager.GetUserAsync(User);

        // Verify user can edit this product
        if (user?.FkSupplierId != product.FkSupplierId)
            return Unauthorized("You can only edit products for your supplier.");

        var vm = ModelToVM(product);

        return View(vm);
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
        var user = await userManager.GetUserAsync(User);

        // Verify user can edit this product
        if (user?.FkSupplierId != product.FkSupplierId)
            return Unauthorized("You can only edit products for your supplier.");

        if (ModelState.IsValid)
        {
            await productRepository.UpdateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        var vm = ModelToVM(product);
        // necessary for <form asp-action="EditProduct" asp-route-id="@ViewBag.ProductId" method="post">
        ViewBag.ProductId = id;
        return View(vm);
    }

    // POST: /Supplier/EditProduct/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProduct(int id, ProductAddOrEditViewModel vm)
    {
        var supplierId = await GetCurrentSupplierIdAsync();
        var existing = await _db.Products.FindAsync(id);
        if (existing == null || existing.FkSupplierId != supplierId)
            return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.ProductId = id;
            return View(vm);
        }

        // Copy editable fields; reset verification so admin must re-approve
        existing.Name = vm.Name;
        existing.ShortName = vm.ShortName;
        existing.Description = vm.Description;
        existing.UnitPrice = vm.UnitPrice;
        existing.StockQuantity = vm.StockQuantity;
        existing.AtomicNumber = vm.AtomicNumber;
        existing.StateOfMatter = vm.StateOfMatter;
        existing.ProductType = vm.ProductType;
        existing.ProductSubtype = vm.ProductSubtype;
        existing.HalfLife = vm.HalfLife;
        existing.Purity = vm.Purity;
        existing.SpecificActivity = vm.SpecificActivity;
        existing.IsAdminVerified = false;

        await _db.SaveChangesAsync();
        TempData["Success"] = "Product updated and is awaiting admin re-verification.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Supplier/DeleteProduct/5
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var supplierId = await GetCurrentSupplierIdAsync();
        var product = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.PkSKU == id);
        if (product == null || product.FkSupplierId != supplierId)
            return NotFound();

        return View(product);
    }

    // POST: /Supplier/DeleteProduct/5
    [HttpPost, ActionName("DeleteProduct")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        // Get current user
        var supplierId = await GetCurrentSupplierIdAsync();

        // Verify user can remove this product
        if (supplierId != product.FkSupplierId)
            return Unauthorized("You can only manage products for your supplier.");

        await productRepository.DeleteAsync(id);
        TempData["Success"] = "Product deleted.";

        return RedirectToAction(nameof(Index));
    }

    public static Product VMToModel(ProductAddOrEditViewModel vm, int? supplierId)
    {
        return new Product
        {
            Name = vm.Name,
            ShortName = vm.ShortName,
            Description = vm.Description,
            UnitPrice = (double)vm.UnitPrice,
            StockQuantity = vm.StockQuantity,
            AtomicNumber = vm.AtomicNumber,
            StateOfMatter = vm.StateOfMatter,
            ProductType = vm.ProductType,
            ProductSubtype = vm.ProductSubtype,
            HalfLife = vm.HalfLife,
            Purity = vm.Purity,
            SpecificActivity = vm.SpecificActivity,
            FkSupplierId = supplierId.Value,
            IsAdminVerified = false,
        };
    }

    public static ProductAddOrEditViewModel ModelToVM(Product p)
    {
        return new ProductAddOrEditViewModel
        {
            Name = p.Name,
            ShortName = p.ShortName,
            Description = p.Description,
            UnitPrice = p.UnitPrice,
            StockQuantity = p.StockQuantity,
            AtomicNumber = p.AtomicNumber,
            StateOfMatter = p.StateOfMatter,
            ProductType = p.ProductType,
            ProductSubtype = p.ProductSubtype,
            HalfLife = p.HalfLife,
            Purity = p.Purity,
            SpecificActivity = p.SpecificActivity,
        };
    }
}
