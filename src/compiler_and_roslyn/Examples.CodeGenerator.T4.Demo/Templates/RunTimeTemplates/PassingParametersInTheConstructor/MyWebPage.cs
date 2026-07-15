namespace Examples.CodeGenerator.T4.Templates.RunTimeTemplates.PassingParametersInTheConstructor;

public partial class MyWebPage
{
    private MyData _data;

    public MyWebPage(MyData data) { _data = data; }
}
