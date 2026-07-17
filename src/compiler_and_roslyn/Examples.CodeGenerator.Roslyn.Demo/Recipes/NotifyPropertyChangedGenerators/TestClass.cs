namespace Examples.CodeGenerator.Roslyn.Recipes.NotifyPropertyChangedGenerators;

/// <summary>
/// Test class for the <see cref="AutoNotifyAttribute"/>.
/// </summary>
/// <remarks>
/// There's an issue that needs to be defined at the top level. It's unlikely to have much impact on the implementation.
/// </remarks>
public partial class TestClass
{
    [AutoNotify]
    private string _testField1 = default!;

    [AutoNotify(PropertyName = "Field2")]
    private int _testField2;
}
