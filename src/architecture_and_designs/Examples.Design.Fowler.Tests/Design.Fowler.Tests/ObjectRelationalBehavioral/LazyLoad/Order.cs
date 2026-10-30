namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad;

/// <summary>
/// A simple domain object that represents an order.
/// </summary>
public class Order
{
    public required int OrderId { get; init; }
    public required string ProductName { get; init; }
}
