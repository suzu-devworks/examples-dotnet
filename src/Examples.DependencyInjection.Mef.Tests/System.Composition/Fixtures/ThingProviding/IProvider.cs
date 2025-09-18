namespace Examples.Tests.System.Composition.Fixtures.ThingProviding;

public interface IProvider<T>
{
    IThing<T, IEnumerable<T>> GetThing();
}
