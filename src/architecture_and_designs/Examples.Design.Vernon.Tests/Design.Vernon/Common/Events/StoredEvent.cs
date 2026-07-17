using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.Common.Events
{
    public class StoredEvent : IEquatable<StoredEvent>
    {
        public StoredEvent(string typeName, DateTime occurredOn, string eventBody, long eventId = -1L)
        {
            AssertionConcern.AssertArgumentNotEmpty(typeName, "The event type name is required.");
            AssertionConcern.AssertArgumentLength(typeName, 100, "The event type name must be 100 characters or less.");

            AssertionConcern.AssertArgumentNotEmpty(eventBody, "The event body is required.");
            AssertionConcern.AssertArgumentLength(eventBody, 65000, "The event body must be 65000 characters or less.");

            this.typeName = typeName;
            this.occurredOn = occurredOn;
            this.eventBody = eventBody;
            this.eventId = eventId;
        }

        private readonly string typeName;

        public string TypeName
        {
            get { return typeName; }
        }

        private readonly DateTime occurredOn;

        public DateTime OccurredOn
        {
            get { return occurredOn; }
        }

        private readonly string eventBody;

        public string EventBody
        {
            get { return eventBody; }
        }

        private readonly long eventId;

        public long EventId
        {
            get { return eventId; }
        }

        public IDomainEvent ToDomainEvent()
        {
            return ToDomainEvent<IDomainEvent>();
        }

        public TEvent ToDomainEvent<TEvent>()
            where TEvent : IDomainEvent
        {
            Type eventType;
            try
            {
                eventType = Type.GetType(this.typeName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Class load error, because: {0}", ex));
            }
            return (TEvent)EventSerializer.Instance.Deserialize(this.eventBody, eventType);
        }

        public bool Equals(StoredEvent other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (object.ReferenceEquals(null, other)) return false;
            return this.eventId.Equals(other.eventId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredEvent);
        }

        public override int GetHashCode()
        {
            return this.eventId.GetHashCode();
        }
    }
}
