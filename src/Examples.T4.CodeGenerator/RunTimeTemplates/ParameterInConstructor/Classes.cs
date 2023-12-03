namespace Examples.T4.CodeGenerator.RunTimeTemplates /* .ParameterInConstructor */;

public class MyDataItem
{
    public string Name { get; init; } = default!;

    public string? Value { get; set; }

}

public class MyData
{
    public IEnumerable<MyDataItem> Items { get; init; } = default!;
}

public partial class MyDataWebPage
{
    private readonly MyData _data;

    public MyDataWebPage(MyData data)
    {
        _data = data;
    }
}
