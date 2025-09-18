namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.ThingProviding;

public class GenericProvider<T>(IThing<T, IEnumerable<T>> thing) : IProvider<T>
{
    private readonly IThing<T, IEnumerable<T>> _thing = thing;

    public IThing<T, IEnumerable<T>> GetThing()
    {
        return _thing;
    }

}
