using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(
        OrderContext orderContext,
        ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation($"Ordering database seeded: {typeof(OrderContext).Name}");
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new()
            {
                Username = "ariel",
                FirstName = "Ariel",
                LastName = "Nulud",
                EmaillAddess = "yellnulud@gmail.com",
                AddressLine = "Rizal",
                Country = "Philippines",
                TotalPrice = 750,
                State = "PH",
                ZipCode = "1860",

                CardName = "Visa",
                CardNumber = "123456789123456",
                CreatedBy = "ariel",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "ariel",
                LastModifiedDate = DateTime.Now
            }
        };
    }
}
