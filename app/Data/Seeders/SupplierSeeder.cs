using Bogus;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class SupplierSeeder(DbContext _context) : BaseSeeder<Supplier>(_context)
{
    private readonly List<string> SeedCurrencyCodes = ["CAD", "CNY", "PLN", "UAH", "USD"];

    public List<Supplier> Generate(int min, int max)
    {
        var fakeSupplierGenerator = new Faker<Supplier>()
            .StrictMode(true)
            .Ignore(s => s.Users)
            .Ignore(s => s.Products)
            .RuleFor(s => s.Id, f => 0)
            .RuleFor(s => s.Name, f => TruncateString(f.Company.CompanyName(), 50))
            .RuleFor(
                s => s.ShortName,
                (f, s) =>
                {
                    var split = s.Name.Split(' ');
                    return TruncateString(string.Join(' ', [split.First(), split.Last()]), 20);
                }
            )
            .RuleFor(s => s.CurrencyCode, f => f.PickRandom(SeedCurrencyCodes));

        var fakeSuppliers = fakeSupplierGenerator.GenerateBetween(min, max);

        foreach (var supplier in fakeSuppliers)
        {
            context.Add(supplier);
        }

        return fakeSuppliers;
    }
}
