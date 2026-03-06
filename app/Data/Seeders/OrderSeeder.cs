using Bogus;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class OrderSeeder(DbContext _context, List<ApplicationUser> _availableUsers)
    : BaseSeeder<Order>(_context)
{
    private readonly List<ApplicationUser> AvailableUsers = _availableUsers;

    public List<Order> Generate(int min, int max)
    {
        var fakeOrderGenerator = new Faker<Order>()
            .StrictMode(true)
            .Ignore(o => o.User)
            .Ignore(o => o.Transaction)
            .Ignore(o => o.OrderLineItems)
            .RuleFor(o => o.PkOrderId, f => 0)
            .RuleFor(o => o.OrderDate, f => f.Date.Past())
            .RuleFor(
                o => o.OrderNumber,
                (f, ol) =>
                    string.Join(
                        "-",
                        ["ORD", ol.OrderDate.Year, f.IndexFaker.ToString().PadLeft(5, '0')]
                    )
            )
            .RuleFor(o => o.Status, f => "FULFILLED")
            .RuleFor(o => o.FkUserId, f => f.PickRandom(AvailableUsers).Id)
            .RuleFor(o => o.FkTransactionId, f => null);

        var fakeOrders = fakeOrderGenerator.GenerateBetween(min, max);

        foreach (var order in fakeOrders)
        {
            context.Add(order);
        }

        return fakeOrders;
    }
}
