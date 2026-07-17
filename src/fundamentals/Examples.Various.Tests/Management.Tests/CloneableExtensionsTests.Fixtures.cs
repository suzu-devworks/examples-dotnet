namespace Examples.Management.Tests;

public partial class CloneableExtensionsTests
{
    // [Serializable] // Used with BinaryFormatter.
    private class NestedClass
    {
        public int Code { get; set; }

        public string? Name { get; set; }
    }

    // [Serializable] // Used with BinaryFormatter.
    private class NonCloneableClass
    {
        public string? Value { get; set; }

        public NestedClass? Nested { get; set; }
    }

    private class CloneableClass : ICloneable
    {
        //ICloneable is considered a bad API now, since it does not specify whether the result is a deep or a shallow copy.
        // I think this is why they do not improve this interface.

        public string? Value { get; set; }

        public NestedClass? Nested { get; set; }

        public object Clone()
        {
            var copied = (CloneableClass)MemberwiseClone();
            //other reference member copy...

            return copied;
        }
    }
}
