namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.LazyInitialization;

public class Customer
{
    private readonly FakeOrderRepository _repository = new FakeOrderRepository();

    // A private field that holds the list of Orders. Initial value is null.
    private List<Order>? _orders;

    public required int Id { get; init; }
    public required string Name { get; init; }

    public List<Order> Orders
    {
        get
        {
            // Load data only if the field is null.
            if (_orders == null)
            {
                _orders = _repository.GetOrdersForCustomer(Id);
            }
            return _orders;
        }
    }

    // Methods to check repository state for testing purposes.
    internal int GetRepositoryCallCount() => _repository.CallCount;
}
