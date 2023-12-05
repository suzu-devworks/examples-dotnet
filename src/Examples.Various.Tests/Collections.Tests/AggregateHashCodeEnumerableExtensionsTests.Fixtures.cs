namespace Examples.Collections.Tests;

public partial class AggregateHashCodeEnumerableExtensionsTests
{

    private class BaseClass
    {
        public int Field1 { get; init; }
        public string? Field2 { get; init; }
        public DateTime? Field3 { get; init; }
    }

    private class DefineWithEffectiveJava : BaseClass
    {
        public override int GetHashCode() =>
            (new object?[] { Field1, Field2, Field3 })
                .AggregateHashCodeWithEffectiveJava();
    }

    private class DefineWithELF : BaseClass
    {
        public override int GetHashCode() =>
            (new object?[] { Field1, Field2, Field3 })
                .AggregateHashCodeWithELF();
    }

    private class DefineWithFNV : BaseClass
    {
        public override int GetHashCode() =>
            (new object?[] { Field1, Field2, Field3 })
                .AggregateHashCodeWithFNV();
    }

    private class DefineWithAggregate : BaseClass
    {
        public override int GetHashCode() =>
            (new object?[] { Field1, Field2, Field3 })
                .AggregateHashCode();
    }

    private class DefineWithCombine : BaseClass
    {
        public override int GetHashCode() =>
            System.HashCode.Combine(Field1, Field2, Field3);

    }

    private class DefineWithAnonymousClass : BaseClass
    {
        public override int GetHashCode() =>
            new { Field1, Field2, Field3 }.GetHashCode();
    }

    private class DefineWithValueTuple : BaseClass
    {
        public override int GetHashCode() =>
            (Field1, Field2, Field3).GetHashCode();
    }

}
