using System.Collections.Generic;
using System.Linq;

#nullable enable
// dotnet_style_prefer_is_null_check_over_reference_equality_method = false
#pragma warning disable IDE0041

namespace Examples.DDD.Dotnet.Domains
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            if (object.ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return object.ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object?> GetEqualityComponents();

        public override bool Equals(object? other)
        {
            if (other == null || other.GetType() != GetType())
            {
                return false;
            }

            var otherValue = (ValueObject)other;

            return this.GetEqualityComponents().SequenceEqual(otherValue.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            return EqualOperator(left, right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right)
        {
            return !EqualOperator(left, right);
        }

    }
}
