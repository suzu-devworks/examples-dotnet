using Examples.CodeGenerator.Roslyn.Introducing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Examples.CodeGenerator.Roslyn.Tests.Introducing;

public class HelloWorldGeneratorTests
{
    [Fact]
    public void Debug()
    {
        var compilation = CSharpCompilation.Create("Hello",
            references:
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            ]);

        var generator = new HelloWorldGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            generator.AsSourceGenerator());

        driver = driver.RunGenerators(compilation, TestContext.Current.CancellationToken);

        var result = driver.GetRunResult();

        Assert.Empty(result.Diagnostics);
        Assert.Single(result.Results);
        Assert.Single(result.Results[0].GeneratedSources);

        var generatedSource = result.Results[0].GeneratedSources[0].SourceText.ToString();
        Assert.Contains("public static class HelloWorld", generatedSource);
    }
}
