using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Repositories;

public class OrderRepository(ApplicationDbContext db)
{
    public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await db
            .Orders.AsNoTracking()
            .Where(o => o.FkUserId == userId)
            .Include(o => o.Transaction)
            .Include(o => o.OrderLineItems)
                .ThenInclude(li => li.Product)
                    .ThenInclude(p => p!.Supplier)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
    {
        return await db
            .Orders.AsNoTracking()
            .Where(o => o.PkOrderId == orderId && o.FkUserId == userId)
            .Include(o => o.Transaction)
            .Include(o => o.OrderLineItems)
                .ThenInclude(li => li.Product)
                    .ThenInclude(p => p!.Supplier)
            .FirstOrDefaultAsync();
    }
}
