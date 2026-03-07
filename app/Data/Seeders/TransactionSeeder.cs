using Bogus;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class TransactionSeeder(DbContext _context, List<Order> _orders)
    : BaseSeeder<Transaction>(_context)
{
    private readonly List<Order> Orders = _orders;

    public new List<Transaction> Generate()
    {
        var fakeTransactionGenerator = new Faker<Transaction>()
            .RuleFor(
                t => t.GatewayTransactionId,
                f => f.Random.String2(17, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            )
            .RuleFor(t => t.GatewayName, f => "PayPal")
            .RuleFor(t => t.CurrencyCode, f => "CAD");

        List<Transaction> transactions = [];

        foreach (var order in Orders)
        {
            var transaction = fakeTransactionGenerator.Generate();

            var subtotal = order.OrderLineItems.Aggregate(
                0d,
                (current, next) =>
                {
                    return current + next.UnitPrice * next.Quantity;
                }
            );

            transaction.Subtotal = subtotal;
            transaction.CombinedTax = subtotal * 0.12d; // Canada PST/GST
            transaction.Total = subtotal + (double)transaction.CombinedTax;

            order.Transaction = transaction;

            context.Add(transaction);
            transactions.Add(transaction);
        }

        return transactions;
    }
}
