using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.Common.Notifications
{
    [Serializable]
    public class Notification : ValueObject
    {
        public Notification(long notificationId, IDomainEvent domainEvent)
        {
            AssertionConcern.AssertArgumentNotNull(domainEvent, "The event is required.");

            this.notificationId = notificationId;
            this.domainEvent = domainEvent;
            this.occurredOn = domainEvent.OccurredOn;
            this.version = domainEvent.EventVersion;
            this.typeName = domainEvent.GetType().FullName;
        }

        private readonly long notificationId;
        private readonly IDomainEvent domainEvent;
        private readonly DateTime occurredOn;
        private readonly string typeName;
        private readonly int version;

        public TEvent GetEvent<TEvent>() where TEvent : IDomainEvent
        {
            return (TEvent)this.domainEvent;
        }

        public long NotificationId
        {
            get { return this.notificationId; }
        }

        public DateTime OccurredOn
        {
            get { return this.occurredOn; }
        }

        public int Version
        {
            get { return this.version; }
        }

        public string TypeName
        {
            get { return this.typeName; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.notificationId;
        }
    }
}
