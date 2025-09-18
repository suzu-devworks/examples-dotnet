using System;

namespace Examples.DependencyInjection;

public abstract class ServiceRegistrationAttribute : Attribute
{
    public Type? ServiceType { get; init; }

    public bool Enabled { get; init; } = true;

}
