namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.TableExample;

public partial class TableTemplate
{
    private readonly IDictionary<string, string> _typeConvertors;

    public TableTemplate(TableInfo table, string @namespace, IDictionary<string, string> typeConvertors)
        => (Table, Namespace, _typeConvertors) = (table, @namespace, typeConvertors);

    public TableInfo Table { get; }
    public string Namespace { get; }

    public string GetColumnType(ColumnInfo column)
        => _typeConvertors.TryGetValue(column.Type, out var n) ? n : column.Type
            + (column.NotNull ? "" : "?");
}
