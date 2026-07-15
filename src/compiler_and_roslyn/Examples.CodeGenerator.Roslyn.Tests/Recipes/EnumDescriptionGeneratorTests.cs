using Examples.CodeGenerator.Roslyn.Recipes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Examples.CodeGenerator.Roslyn.Tests.Recipes;

public class EnumDescriptionGeneratorTests
{
    [Fact]
    public void Debug()
    {
        const string source = """
            public class TestClass
            {
                [Examples.CodeGenerator.Generated.GenerateDescriptionAttribute]
                public enum TestEnum
                {
                    /// <summary>
                    /// FirstValue
                    /// summary.
                    /// </summary>
                    FirstValue,
                }
            }
            """;

        var syntaxTree = CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ]);

        var generator = new EnumDescriptionGenerator();

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
        Assert.Contains("internal sealed class GenerateDescriptionAttribute", attributeSource);

        var generatedSource = result.Results[0].GeneratedSources[2].SourceText.ToString();
        Assert.Contains("public static class TestClass_TestEnumGeneratedExtensions", generatedSource);
    }

}
