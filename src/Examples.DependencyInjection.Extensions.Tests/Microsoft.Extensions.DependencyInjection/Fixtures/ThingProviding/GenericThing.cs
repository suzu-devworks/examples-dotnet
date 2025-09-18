namespace Examples.Tests.Microsoft.Extensions.DependencyInjection.Fixtures.ThingProviding;

public class GenericThing<T1, T2>(IVerifier verifier) : IThing<T1, T2>
{
    private readonly IVerifier _verifier = verifier;

    public string Name => $"Type of {typeof(T1).Name},{typeof(T2).Name}";

    public T1? Value => default;

    public void Exec(T2 element)
    {
        _verifier.Called($"Name is {Name}.");
        _verifier.Called($"Value is {Value}.");
    }
}
