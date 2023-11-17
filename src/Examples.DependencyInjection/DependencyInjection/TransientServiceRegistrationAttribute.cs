using System;

namespace Examples.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientServiceRegistrationAttribute : ServiceRegistrationAttribute
{
}
