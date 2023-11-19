using System;

namespace Examples.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonServiceRegistrationAttribute : ServiceRegistrationAttribute
{
}
