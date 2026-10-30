namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.ValueHolders;

public class Customer
{
    public int Id { get; }
    public string? Name { get; set; }

    // The Orders property is a ValueHolder.
    public ValueHolder<List<Order>> Orders { get; private set; }

    private readonly FakeOrderRepository _repository;

    public Customer(int id)
    {
        Id = id;
        _repository = new FakeOrderRepository();

        // Initialize the ValueHolder by passing the data loading logic to it.
        Orders = new ValueHolder<List<Order>>(() => _repository.GetOrdersForCustomer(Id));
    }

    internal int GetRepositoryCallCount() => _repository.CallCount;
}
