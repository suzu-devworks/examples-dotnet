namespace Examples.Tests.System.Composition.Fixtures.ThingProviding;

public class GenericProvider<T>(IThing<T, IEnumerable<T>> thing) : IProvider<T>
{
    private readonly IThing<T, IEnumerable<T>> _thing = thing;

    public IThing<T, IEnumerable<T>> GetThing()
    {
        return _thing;
    }

}
