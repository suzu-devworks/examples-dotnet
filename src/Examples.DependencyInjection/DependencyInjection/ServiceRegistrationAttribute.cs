using System;

namespace Examples.DependencyInjection;

public abstract class ServiceRegistrationAttribute : Attribute
{
    public Type ServiceType { get; init; } = default!;

    public bool Enabled { get; init; } = true;

}
