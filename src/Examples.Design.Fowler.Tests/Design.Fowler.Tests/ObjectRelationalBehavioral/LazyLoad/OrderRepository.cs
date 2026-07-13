
namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad;

/// <summary>
/// A repository that simulates database access.
/// </summary>
public class FakeOrderRepository
{
    public int CallCount { get; private set; }

    // Get a list of orders for a specific customer ID (instead of a database access).
    public List<Order> GetOrdersForCustomer(int customerId)
    {
        // Record that this method was called.
        CallCount++;
        Console.WriteLine($"---> DATABASE HIT: Loading orders for customer {customerId}...");

        // Returning sample data.
        return new List<Order>
        {
            new Order { OrderId = 101, ProductName = "Book" },
            new Order { OrderId = 102, ProductName = "Laptop" }
        };
    }
}
