using Bogus;
using Microsoft.EntityFrameworkCore;
using SSD2600_CDEGP.Models;

namespace SSD2600_CDEGP.Data.Seeders;

public class OrderLineItemSeeder(
    DbContext _context,
    List<Order> _availableOrders,
    List<Product> _availableProduct
) : BaseSeeder<OrderLineItem>(_context)
{
    private readonly List<Order> AvailableOrders = _availableOrders;
    private readonly List<Product> AvailableProducts = _availableProduct;

    public List<OrderLineItem> Generate(int min, int max)
    {
        var faker = new Faker();
        var fakeOrderLineItemGenerator = new Faker<OrderLineItem>().RuleFor(
            o => o.Quantity,
            f => f.Random.Int(1, 20)
        );

        List<OrderLineItem> orderLineItems = [];

        foreach (var order in AvailableOrders)
        {
            int productsToPick = faker.Random.Int(1, 5);

            var pickedProducts = faker.PickRandom(AvailableProducts, productsToPick);

            foreach (var product in pickedProducts)
            {
                var lineItem = fakeOrderLineItemGenerator.Generate();
                lineItem.FkOrderId = order.PkOrderId;
                lineItem.FkProductSKU = product.PkSKU;
                lineItem.UnitPrice = product.UnitPrice;

                // Simulate discounted units
                if (faker.Random.Decimal() > 0.8m)
                {
                    lineItem.UnitPrice *= (double)faker.Random.Decimal(min: 0.5m);
                }

                context.Add(lineItem);
                orderLineItems.Add(lineItem);
            }
        }

        return orderLineItems;
    }
}
