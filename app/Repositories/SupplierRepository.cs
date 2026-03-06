using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Repositories;

public class SupplierRepository(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context
            .Suppliers.Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers.Include(s => s.Products).ToListAsync();
    }

    public async Task AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
