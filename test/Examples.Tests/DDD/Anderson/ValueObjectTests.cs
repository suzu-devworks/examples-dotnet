using System;
using ChainingAssertion;
using Examples.DDD.Anderson.Domains;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DDD.Anderson
{
    public class ValueObjectTests
    {
        private sealed class Delived : ValueObject<Delived>
        {
            public int Id { get; init; }
            public string Name { get; init; }

            public DateTime ExpiredAt { get; init; }

            protected override bool EqualsCore(Delived other)
            {
                if (other == null) { return false; }
                if (object.ReferenceEquals(other, this)) { return true; }

                if (!Equals(this.Id, other.Id)) { return false; }
                if (!Equals(this.Name, other.Name)) { return false; }
                if (!Equals(this.ExpiredAt, other.ExpiredAt)) { return false; }

                return true;
            }
        }

        [Fact]
        void TestEquals()
        {
            var object1 = new Delived() { Id = 1, Name = "ID=1", ExpiredAt = DateTime.MaxValue };

            object1.Equals(new Delived() { Id = 1, Name = "ID=1", ExpiredAt = DateTime.MaxValue }).Is(true);
            (object1 == new Delived() { Id = 1, Name = "ID=1", ExpiredAt = DateTime.MaxValue }).Is(true);
            object1.Equals(null).Is(false);
            (object1 == null).Is(false);

            return;
        }

        [Fact]
        void TestNotEquals()
        {
            var object1 = new Delived() { Id = 1, Name = "ID=1", ExpiredAt = DateTime.MaxValue };

            (object1 != new Delived() { Id = 2, Name = "ID=1", ExpiredAt = DateTime.MaxValue }).Is(true);
            (object1 != new Delived() { Id = 1, Name = "ID=2", ExpiredAt = DateTime.MaxValue }).Is(true);
            (object1 != new Delived() { Id = 2, Name = "ID=1", ExpiredAt = DateTime.Now }).Is(true);
            (object1 != new Delived() { Id = 1, Name = "ID=1" }).Is(true);
            (object1 != null).Is(true);

            return;
        }
    }
}
