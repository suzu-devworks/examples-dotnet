namespace Examples.DependencyInjection;

public abstract class ServiceRegistoartionAttribute : Attribute
{
    public Type ServiceType { get; init; } = default!;

    public bool Enabled { get; init; } = true;

}
