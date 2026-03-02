using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class ApplicationUserSeeder(DbContext _context, List<Supplier> _availableSuppliers)
    : BaseSeeder<ApplicationUser>(_context)
{
    private readonly List<Supplier> suppliers = _availableSuppliers;

    public List<ApplicationUser> Generate(int min, int max)
    {
        Generate();

        var passHasher = new PasswordHasher<ApplicationUser>();
        var fakeApplicationUserGenerator = new Faker<ApplicationUser>()
            .RuleFor(s => s.Email, f => f.Internet.Email())
            .RuleFor(s => s.NormalizedEmail, (f, u) => u.Email?.ToUpperInvariant())
            .RuleFor(s => s.EmailConfirmed, f => true)
            // IdentityFW does login based on UserName but not Email even though
            // the scaffolded Login/register views ask for an Email???? This
            // makes zero sense to me
            .RuleFor(s => s.UserName, (f, u) => u.Email)
            .RuleFor(s => s.NormalizedUserName, (f, u) => u.NormalizedEmail)
            .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(s => s.PhoneNumberConfirmed, f => true)
            // Simple password for testing purposes
            .RuleFor(s => s.PasswordHash, (f, u) => passHasher.HashPassword(u, "password"))
            // Chance for user to be associated with supplier
            .RuleFor(
                u => u.FkSupplierId,
                f =>
                {
                    var s = f.PickRandom(suppliers).OrNull(f, 0.5f);
                    return s?.Id;
                }
            )
            // IdentityFW stuff
            .RuleFor(u => u.SecurityStamp, f => Guid.NewGuid().ToString())
            .RuleFor(u => u.ConcurrencyStamp, f => Guid.NewGuid().ToString());

        var fakeApplicationUsers = fakeApplicationUserGenerator.GenerateBetween(min, max);

        foreach (var user in fakeApplicationUsers)
        {
            context.Add(user);
        }

        return fakeApplicationUsers;
    }
}
