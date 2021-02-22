using System;
using System.Collections.Generic;
using ChainingAssertion;
using Examples.DDD.Dotnet.Domains;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.DDD.Dotnet
{
    public class ValueObjectTests
    {
        private sealed class Delived : ValueObject
        {
            public int Id { get; init; }
            public string Name { get; init; }

            public DateTime ExpiredAt { get; init; }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Id;
                yield return Name;
                yield return ExpiredAt;
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
