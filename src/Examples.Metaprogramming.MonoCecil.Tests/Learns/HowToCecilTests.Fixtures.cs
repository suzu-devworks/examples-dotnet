namespace Examples.Metaprogramming.MonoCecil.Tests.Learns
{
    namespace Foo
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class IgnoreAttribute(string message) : Attribute
        {
            public string Message => message;
        }
    }

    public partial class HowToCecilTests
    {
        [Foo.Ignore("Not working yet")]
        public class Fixture(int x)
        {
            public int Sum(int y)
            {
                return x + y;
            }
        }
    }

};
