namespace Examples.Collections.Tests;

/// <summary>
/// Tests <see cref="AggregateHashCodeEnumerableExtensions" /> methods.
/// </summary>
public partial class AggregateHashCodeEnumerableExtensionsTests
{

    [Theory]
    [MemberData(nameof(DataOfEqualHashCode))]
    public void WhenCallingGetHashCode_WithSameValuesToObject_ReturnsSameHash(object object1, object object2, string reason)
    {
        (object1.GetHashCode() == object2.GetHashCode()).IsTrue(reason);

        return;
    }

    public static readonly TheoryData<object, object, string> DataOfEqualHashCode = new()
    {
        {
            new DefineWithEffectiveJava() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithEffectiveJava() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithEffectiveJava)}."
        },
        {
            new DefineWithELF() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithELF() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithELF)}."
        },
        {
            new DefineWithFNV() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithFNV() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithFNV)}."
        },
        {
            new DefineWithAggregate() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithAggregate() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithAggregate)}."
        },
        {
            new DefineWithCombine() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithCombine() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithCombine)}."
        },
        {
            new DefineWithAnonymousClass() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithAnonymousClass() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithAnonymousClass)}."
        },
        {
            new DefineWithValueTuple() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            new DefineWithValueTuple() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithValueTuple)}."
        },
    };


    [Theory]
    [MemberData(nameof(DataOfNotEqualHashCode))]
    public void WhenCallingGetHashCode_WithDifferentValuesToObject_ReturnsDifferentHash(object object1, object object2, string reason)
    {
        (object1.GetHashCode() != object2.GetHashCode()).IsTrue(reason);

        return;
    }

    public static readonly TheoryData<object, object, string> DataOfNotEqualHashCode = new()
    {
        {
            new DefineWithEffectiveJava() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithEffectiveJava() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithEffectiveJava)}."
        },
        {
            new DefineWithELF() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithELF() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithELF)}."
        },
        {
            new DefineWithFNV() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithFNV() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithFNV)}."
        },
        {
            new DefineWithAggregate() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithAggregate() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithAggregate)}."
        },
        {
            new DefineWithCombine() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithCombine() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithCombine)}."
        },
        {
            new DefineWithAnonymousClass() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithAnonymousClass() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be not equal because implement GetHashCode in {nameof(DefineWithAnonymousClass)}."
        },
        {
            new DefineWithValueTuple() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-28") },
            new DefineWithValueTuple() { Field1 = 123, Field2 = "Hoge", Field3 = DateTime.Parse("2020-02-29") },
            $"they should be equal because implement GetHashCode in {nameof(DefineWithValueTuple)}."
        },
    };

}
