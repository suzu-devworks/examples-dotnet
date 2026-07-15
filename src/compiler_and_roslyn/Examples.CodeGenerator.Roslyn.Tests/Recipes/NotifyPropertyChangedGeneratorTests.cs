using Examples.CodeGenerator.Roslyn.Recipes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Examples.CodeGenerator.Roslyn.Tests.Recipes;

public class NotifyPropertyChangedGeneratorTests
{
    [Fact]
    public void Debug()
    {
        const string source = """
            using Examples.CodeGenerator.Generated;

            namespace TestNamespace
            {
                public partial class TestClass
                {
                    [AutoNotify]
                    private string _testField1, _b;

                    [AutoNotify(PropertyName = "Field2")]
                    private string _testField2;
                }
            }
            """;

        var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.INotifyPropertyChanged).Assembly.Location),
            ]);

        var generator = new NotifyPropertyChangedGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generator.AsSourceGenerator());

        driver = driver.RunGenerators(compilation, TestContext.Current.CancellationToken);

        var result = driver.GetRunResult();

        Assert.Empty(result.Diagnostics);
        Assert.Single(result.Results);
        Assert.Equal(3, result.Results[0].GeneratedSources.Length);

        var embeddedSource = result.Results[0].GeneratedSources[0].SourceText.ToString();
        Assert.Contains("internal sealed partial class EmbeddedAttribute", embeddedSource);

        var attributeSource = result.Results[0].GeneratedSources[1].SourceText.ToString();
        Assert.Contains("internal sealed class AutoNotifyAttribute", attributeSource);

        var generatedSource = result.Results[0].GeneratedSources[2].SourceText.ToString();
        Assert.Contains("public partial class TestClass", generatedSource);
    }

}
