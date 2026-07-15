namespace Examples.Metaprogramming.ExpressionTrees.Tests.Learns;

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
