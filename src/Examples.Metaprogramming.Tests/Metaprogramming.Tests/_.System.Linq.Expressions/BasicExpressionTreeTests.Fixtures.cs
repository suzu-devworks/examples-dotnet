namespace Examples.Metaprogramming.Tests._.System.Linq.Expressions;

public partial class BasicExpressionTreeTests
{

    private class Animal
    {
        public Animal() : this("Unknown")
        {
        }

        public Animal(string name) => Name = name;

        public string? Name { get; init; }
    }

}
