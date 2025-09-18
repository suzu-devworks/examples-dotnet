namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.ThingProviding;

public interface IProvider<T>
{
    IThing<T, IEnumerable<T>> GetThing();
}
