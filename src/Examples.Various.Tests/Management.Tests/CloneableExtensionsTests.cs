namespace Examples.Management.Tests;

/// <summary>
/// Tests <see cref="CloneableExtensions" /> methods.
/// </summary>
public partial class CloneableExtensionsTests
{
    public class DeepCopyMethod
    {
        [Fact]
        public void When_CopingIncludingNestedClasses_Then_ReturnsDifferentInstanceWithSameValue()
        {
            var original = new NonCloneableClass() { Value = "123", Nested = new() { Code = 123, Name = "foo" } };

            var copied = original.DeepCopy();

            // clone object is another instance.
            Assert.False(copied == original);
            Assert.False(ReferenceEquals(original, copied));

            // member is deep copies.
            Assert.False(ReferenceEquals(original.Value, copied.Value));
            Assert.True(copied.Value == original.Value);
            Assert.False(ReferenceEquals(original.Nested, copied.Nested));
            Assert.True(copied.Nested!.Code == original.Nested!.Code);
            Assert.True(copied.Nested!.Name == original.Nested!.Name);
        }
    }

    public class ShallowCopyMethod
    {
        [Fact]
        public void When_CopingIncludingNestedClasses_Then_ReturnsAsReferenceCopy()
        {
            var original = new NonCloneableClass() { Value = "123", Nested = new() { Code = 123, Name = "foo" } };

            var copied = original.ShallowCopy();

            // clone object is another instance.
            Assert.False(copied == original);
            Assert.False(ReferenceEquals(original, copied));

            // member is shallow copies.
            Assert.True(copied.Value == original.Value);
            Assert.True(ReferenceEquals(original.Value, copied.Value));
            Assert.True(copied.Nested!.Code == original.Nested!.Code);
            Assert.True(copied.Nested!.Name == original.Nested!.Name);
            Assert.True(ReferenceEquals(original.Nested, copied.Nested));
        }

        [Fact]
        public void When_CopingCloneableClass_Then_ReturnsAsReferenceCopy()
        {
            var original = new CloneableClass() { Value = "123", Nested = new() { Code = 123, Name = "foo" } };

            var copied = (CloneableClass)original.Clone();

            // clone object is another instance.
            Assert.False(copied == original);
            Assert.False(ReferenceEquals(original, copied));

            // member is shallow copies.
            Assert.True(copied.Value == original.Value);
            Assert.True(ReferenceEquals(original.Value, copied.Value));
            Assert.True(copied.Nested!.Code == original.Nested!.Code);
            Assert.True(copied.Nested!.Name == original.Nested!.Name);
            Assert.True(ReferenceEquals(original.Nested, copied.Nested));
        }
    }
}
