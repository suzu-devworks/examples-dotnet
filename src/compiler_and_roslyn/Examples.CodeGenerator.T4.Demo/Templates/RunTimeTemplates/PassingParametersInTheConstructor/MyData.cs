namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.PassingParametersInTheConstructor;

public class MyData
{
    public IEnumerable<MyDataItem> Items { get; init; } = default!;
}

public class MyDataItem
{
    public string Name { get; init; } = default!;

    public string? Value { get; set; }

}

