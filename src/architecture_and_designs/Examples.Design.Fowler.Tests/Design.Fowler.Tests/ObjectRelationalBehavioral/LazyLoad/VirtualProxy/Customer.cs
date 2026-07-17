namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.VirtualProxy;

// Common interface implemented by Customer.
public interface ICustomer
{
    int Id { get; }
    string Name { get; }
    List<Order> Orders { get; }
    int GetRepositoryCallCount();
}

// A real Customer object (data loading is done in the constructor).
public class RealCustomer : ICustomer
{
    private readonly FakeOrderRepository _repository = new FakeOrderRepository();
    public int Id { get; }
    public string Name { get; }
    public List<Order> Orders { get; }

    public RealCustomer(int id)
    {
        Id = id;
        Name = $"Real Customer {id}"; // Originally, this was also obtained from the DB.
        Orders = _repository.GetOrdersForCustomer(Id);
    }

    public int GetRepositoryCallCount() => _repository.CallCount;
}

// Customer proxy object.
public class CustomerProxy : ICustomer
{
    private readonly int _id;
    private RealCustomer? _realCustomer; // The field that holds the real object.

    public CustomerProxy(int id)
    {
        _id = id;
    }

    public int Id => _id;
    public string Name => GetRealCustomer().Name;
    public List<Order> Orders => GetRealCustomer().Orders;
    public int GetRepositoryCallCount() => _realCustomer?.GetRepositoryCallCount() ?? 0;

    // Only create the actual object when it is needed.
    private RealCustomer GetRealCustomer()
    {
        if (_realCustomer == null)
        {
            _realCustomer = new RealCustomer(_id);
        }
        return _realCustomer;
    }
}
