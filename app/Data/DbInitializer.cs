using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Data;
using SSD2600_CDEGP.Data.Seeders;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context, bool doReseed = false)
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

            context.Products.ExecuteDelete();
            context.Suppliers.ExecuteDelete();

            context.ContactDetail.ExecuteDelete();
        }

        context.Database.EnsureCreated();

        if (!doReseed && context.Users.Any())
        {
            return;
        }

        var fakeSuppliers = new SupplierSeeder(context).Generate(10, 20);
        context.SaveChanges();

        var fakeUsers = new ApplicationUserSeeder(context, fakeSuppliers).Generate(20, 50);
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
        context.SaveChanges();
    }
}
