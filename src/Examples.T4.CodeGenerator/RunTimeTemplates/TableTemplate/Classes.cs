namespace Examples.T4.CodeGenerator.RunTimeTemplates /* .TableTemplate */;

public class TableInfo
{
    public string Name { get; set; } = default!;

    public IEnumerable<ColumnInfo> Columns { get; set; } = default!;

    public string? Description { get; set; }

}

public class ColumnInfo
{
    public string Name { get; set; } = default!;

    public string Type { get; set; } = default!;

    public bool IsPrimary { get; set; }

    public bool NotNull { get; set; }

    public string? Description { get; set; }

}

public partial class TextTemplate : ITemplate
{
    private readonly Dictionary<string, string> _typeDictionary;

    public string NameSpace { get; }

    public TableInfo Table { get; }

    public TextTemplate(Dictionary<string, string> typeDictionary, string nameSpace, TableInfo table)
        => (_typeDictionary, NameSpace, Table) = (typeDictionary, nameSpace, table);

    public string GetColumnType(ColumnInfo column)
        => _typeDictionary.TryGetValue(column.Type, out var n) ? n : column.Type
            + (column.NotNull ? "" : "?");
}
