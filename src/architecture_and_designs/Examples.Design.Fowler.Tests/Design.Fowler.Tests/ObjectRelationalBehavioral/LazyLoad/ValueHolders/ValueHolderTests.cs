namespace Examples.Design.Fowler.Tests.ObjectRelationalBehavioral.LazyLoad.ValueHolders;

/// <summary>
/// Pattern 3: Value Holder.
/// </summary>
/// <remarks>
/// <para>Generated with Google Gemini Pro 2.5.</para>
/// </remarks>
public class ValueHolderTests
{
    [Fact]
    public void When_AccessingValueHolder_Then_LoadsValueOnlyOnFirstAccess()
    {
        // Arrange
        var customer = new Customer(3);

        // Assert: No DB access occurs immediately after creating a Customer object.
        Assert.Equal(0, customer.GetRepositoryCallCount());

        // Act: Accessing the Value property of a ValueHolder for the first time.
        var orders = customer.Orders.Value;

        // Assert: The first access will result in one DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal(2, orders.Count);

        // Act: Second.
        var ordersAgain = customer.Orders.Value;

        // Assert: The second access does not increase DB access.
        Assert.Equal(1, customer.GetRepositoryCallCount());
        Assert.Equal(2, ordersAgain.Count);
    }
}
