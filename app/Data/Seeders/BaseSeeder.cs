using Microsoft.EntityFrameworkCore;

namespace SSD2600_CDEGP.Data.Seeders;

public class BaseSeeder<T>(DbContext _context)
    where T : class
{
    protected readonly DbContext dbContext = _context;
    protected readonly DbSet<T> context = _context.Set<T>();

    protected static string TruncateString(string str, int max)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        return str.Length <= max ? str : str[..max];
    }

    public List<T> Generate()
    {
        return [];
    }
}
