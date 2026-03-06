using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Repositories;

public class ProductRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context
            .Products.Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.PkSKU == id);
    }

    public async Task<IEnumerable<Product>> GetBySupplierIdAsync(int supplierId)
    {
        return await _context.Products.Where(p => p.FkSupplierId == supplierId).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.Include(p => p.Supplier).ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
