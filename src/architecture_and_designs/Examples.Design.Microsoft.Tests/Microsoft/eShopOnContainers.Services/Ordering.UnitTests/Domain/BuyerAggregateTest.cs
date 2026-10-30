using System;
using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.Exceptions;

namespace Microsoft.eShopOnContainers.Services.Ordering.UnitTests.Domain;

public class BuyerAggregateTest
{
    public BuyerAggregateTest()
    { }

    [Fact]
    public void When_CreatingBuyer_WithValidIdentity_Then_ReturnsInstance()
    {
        //Arrange
        var identity = new Guid().ToString();
        var name = "fakeUser";

        //Act
        var fakeBuyerItem = new Buyer(identity, name);

        //Assert
        Assert.NotNull(fakeBuyerItem);
    }

    [Fact]
    public void When_CreatingBuyer_WithEmptyIdentity_Then_ThrowsArgumentNullException()
    {
        //Arrange
        var identity = string.Empty;
        var name = "fakeUser";

        //Act - Assert
        Assert.Throws<ArgumentNullException>(() => new Buyer(identity, name));
    }

    [Fact]
    public void When_AddingPaymentMethod_Then_ReturnsPaymentMethod()
    {
        //Arrange
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);
        var orderId = 1;
        var name = "fakeUser";
        var identity = new Guid().ToString();
        var fakeBuyerItem = new Buyer(identity, name);

        //Act
        var result = fakeBuyerItem.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration, orderId);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void When_CreatingPaymentMethod_WithValidData_Then_ReturnsInstance()
    {
        //Arrange
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);
        _ = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Act
        var result = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void When_CreatingPaymentMethod_WithExpiredDate_Then_ThrowsOrderingDomainException()
    {
        //Arrange
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(-1);

        //Act - Assert
        Assert.Throws<OrderingDomainException>(() => new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration));
    }

    [Fact]
    public void When_ComparingPaymentMethodEquality_Then_ReturnsTrue()
    {
        //Arrange
        var cardTypeId = 1;
        var alias = "fakeAlias";
        var cardNumber = "124";
        var securityNumber = "1234";
        var cardHolderName = "FakeHolderNAme";
        var expiration = DateTime.Now.AddYears(1);

        //Act
        var fakePaymentMethod = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
        var result = fakePaymentMethod.IsEqualTo(cardTypeId, cardNumber, expiration);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void When_AddingPaymentMethod_Then_RaisesDomainEvent()
    {
        //Arrange
        var alias = "fakeAlias";
        var orderId = 1;
        var cardTypeId = 5;
        var cardNumber = "12";
        var cardSecurityNumber = "123";
        var cardHolderName = "FakeName";
        var cardExpiration = DateTime.Now.AddYears(1);
        var expectedResult = 1;
        var name = "fakeUser";

        //Act
        var fakeBuyer = new Buyer(Guid.NewGuid().ToString(), name);
        fakeBuyer.VerifyOrAddPaymentMethod(cardTypeId, alias, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, orderId);

        //Assert
        Assert.Equal(fakeBuyer.DomainEvents.Count, expectedResult);
    }
}
