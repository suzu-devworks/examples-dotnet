namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.Ghosts;

public class Customer
{
    public int Id { get; }
    private bool _isGhost = true;
    private readonly FakeOrderRepository _repository = new FakeOrderRepository();

    private string? _name;
    public string Name
    {
        get
        {
            Load(); // Attempt to load on access.
            return _name!;
        }
    }

    private List<Order>? _orders;
    public List<Order> Orders
    {
        get
        {
            Load(); // Attempt to load on access.
            return _orders!;
        }
    }

    // Initialize as a ghost with ID only.
    public Customer(int id)
    {
        Id = id;
    }

    // Method to load all data.
    private void Load()
    {
        if (_isGhost)
        {
            // Here we retrieve all data such as Name and Orders from the DB.
            // Normally, all data would be retrieved in one DB access, 
            // but here we will retrieve Orders instead.
            _orders = _repository.GetOrdersForCustomer(Id);
            _name = $"Ghost Customer {Id}";

            _isGhost = false;
        }
    }

    public int GetRepositoryCallCount() => _repository.CallCount;
}
