using System;

namespace Examples.Design.Anderson.ValueObjects.Tests;

public partial class ValueObjectTests
{

    public sealed class Derived : ValueObject<Derived>
    {
        public int Id { get; init; }
        public string? Name { get; init; }

        public DateTime ExpiredAt { get; init; }

        protected override bool EqualsCore(Derived? other)
        {
            if (other == null) { return false; }
            if (object.ReferenceEquals(other, this)) { return true; }

            if (!Equals(this.Id, other.Id)) { return false; }
            if (!Equals(this.Name, other.Name)) { return false; }
            if (!Equals(this.ExpiredAt, other.ExpiredAt)) { return false; }

            return true;
        }
    }

}
