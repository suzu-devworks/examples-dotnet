using System;

namespace Examples
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ExcludeAttribute : Attribute
    {
    }
}
