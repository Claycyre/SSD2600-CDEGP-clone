using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Data.Seeders;
using SSD2600_CDEGP.Models;

public static class DbInitializer
{
    public static void Seed(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        bool doReseed = false
    )
    {
        if (doReseed)
        {
            context.OrderLineItems.ExecuteDelete();
            context.Orders.ExecuteDelete();
            context.Transactions.ExecuteDelete();

            // User teardown
            context.UserTokens.ExecuteDelete();
            context.UserLogins.ExecuteDelete();
            context.UserClaims.ExecuteDelete();
            context.UserRoles.ExecuteDelete();
            context.Users.ExecuteDelete();

            context.Roles.ExecuteDelete();

            context.Products.ExecuteDelete();
            context.Suppliers.ExecuteDelete();

            context.ContactDetail.ExecuteDelete();
        }

        context.Database.Migrate();

        // Ensure required roles exist
        foreach (var role in new[] { "Admin", "Supplier" })
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }

        // Seed admin account if it doesn't exist
        if (userManager.FindByNameAsync("admin@admin.com").GetAwaiter().GetResult() == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                UserRole = "SiteAdmin",
                RegisteredAt = DateTime.UtcNow,
                VerificationSubmitted = true,
                VerificationApproved = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(
                adminUser,
                "admin"
            );
            userManager.CreateAsync(adminUser).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }

        // Seed admin account if it doesn't exist
        if (
            userManager.FindByNameAsync("admin@prometheusatomics.local").GetAwaiter().GetResult()
            == null
        )
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@prometheusatomics.local",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@prometheusatomics.local",
                NormalizedEmail = "ADMIN@PROMETHEUS.LOCAL",
                EmailConfirmed = true,
                UserRole = "SiteAdmin",
                RegisteredAt = DateTime.UtcNow,
                VerificationSubmitted = true,
                VerificationApproved = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(
                adminUser,
                "admin"
            );
            userManager.CreateAsync(adminUser).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }

        // Seed admin account if it doesn't exist
        if (userManager.FindByNameAsync("admin@admin.com").GetAwaiter().GetResult() == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                UserRole = "SiteAdmin",
                RegisteredAt = DateTime.UtcNow,
                VerificationSubmitted = true,
                VerificationApproved = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(
                adminUser,
                "endmin"
            );
            userManager.CreateAsync(adminUser).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }

        if (!doReseed && context.Users.Count() > 2)
        {
            return;
        }

        var fakeSuppliers = new SupplierSeeder(context).Generate(10, 20);
        context.SaveChanges();

        var fakeUsers = new ApplicationUserSeeder(context, fakeSuppliers).Generate(20, 50);

        // Seed a few named supplier user accounts linked to the first suppliers
        var passHasher = new PasswordHasher<ApplicationUser>();
        var supplierAccounts = new List<ApplicationUser>();
        var namedSuppliers = fakeSuppliers.Take(3).ToList();
        var supplierEmails = new[]
        {
            "supplier1@example.com",
            "supplier2@example.com",
            "supplier3@example.com",
        };
        for (int i = 0; i < namedSuppliers.Count; i++)
        {
            var email = supplierEmails[i];
            if (userManager.FindByNameAsync(email).GetAwaiter().GetResult() == null)
            {
                var supplierUser = new ApplicationUser
                {
                    UserName = email,
                    NormalizedUserName = email.ToUpperInvariant(),
                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),
                    EmailConfirmed = true,
                    UserRole = "Supplier",
                    FkSupplierId = namedSuppliers[i].Id,
                    RegisteredAt = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };
                supplierUser.PasswordHash = passHasher.HashPassword(supplierUser, "supplier");
                userManager.CreateAsync(supplierUser).GetAwaiter().GetResult();
                userManager.AddToRoleAsync(supplierUser, "Supplier").GetAwaiter().GetResult();
                supplierAccounts.Add(supplierUser);
            }
        }

        context.SaveChanges();

        new ContactDetailSeeder(context, fakeUsers).Generate();
        context.SaveChanges();

        var fakeProducts = new ProductSeeder(context, fakeSuppliers).Generate();
        context.SaveChanges();

        var fakeOrders = new OrderSeeder(context, fakeUsers).Generate(100, 200);
        context.SaveChanges();

        new OrderLineItemSeeder(context, fakeOrders, fakeProducts).Generate(100, 200);
        context.SaveChanges();

        new TransactionSeeder(context, fakeOrders).Generate();
    }
}
