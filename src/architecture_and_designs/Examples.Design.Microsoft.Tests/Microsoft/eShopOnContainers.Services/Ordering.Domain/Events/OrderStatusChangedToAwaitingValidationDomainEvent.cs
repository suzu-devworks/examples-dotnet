namespace Ordering.Domain.Events
{
    using System.Collections.Generic;
    using MediatR;
    using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;

    /// <summary>
    /// Event used when the grace period order is confirmed
    /// </summary>
    public class OrderStatusChangedToAwaitingValidationDomainEvent
         : INotification
    {
        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId,
            IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }
    }
}
