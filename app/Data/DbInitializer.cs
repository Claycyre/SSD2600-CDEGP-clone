using Bogus;
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

        if (!doReseed && context.Users.Any())
        {
            return;
        }

        var fakeSuppliers = new SupplierSeeder(context).Generate(10, 20);
        context.SaveChanges();

        var userSeeder = new ApplicationUserSeeder(context, fakeSuppliers);
        var protoUsers = new List<ProtoUser>
        {
            new()
            {
                User = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    UserRole = "SiteAdmin",
                },
                Password = "admin",
                AddUserToRole = "Admin",
            },
            new()
            {
                User = new ApplicationUser
                {
                    UserName = "admin@prometheusatomics.local",
                    Email = "admin@prometheusatomics.local",
                    UserRole = "SiteAdmin",
                },
                Password = "admin",
                AddUserToRole = "Admin",
            },
        };

        for (int i = 0; i < Math.Floor((double)fakeSuppliers.Count / 3); i++)
        {
            var email = string.Concat("supplier", (i + 1).ToString(), "@example.com");
            protoUsers.Add(
                new ProtoUser
                {
                    User = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        UserRole = "Supplier",
                        FkSupplierId = fakeSuppliers[i].Id,
                    },
                    Password = "supplier",
                    AddUserToRole = "Supplier",
                }
            );
        }
        userSeeder.SeedThese(protoUsers);

        var fakeUsers = userSeeder.Generate(20, 50);
        context.SaveChanges();

        foreach (var protoUser in userSeeder.GetPrescribedUsers())
        {
            if (!string.IsNullOrEmpty(protoUser.AddUserToRole))
            {
                userManager
                    .AddToRoleAsync(protoUser.User, protoUser.AddUserToRole)
                    .GetAwaiter()
                    .GetResult();
            }
        }

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
