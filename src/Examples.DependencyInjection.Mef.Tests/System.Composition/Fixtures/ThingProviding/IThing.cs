namespace Examples.Tests.System.Composition.Fixtures.ThingProviding;

public interface IThing<T1, T2>
{
    string Name { get; }

    T1? Value { get; }

    void Exec(T2 element);
}
