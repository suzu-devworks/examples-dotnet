using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Examples.Analyzer.Roslyn.Tests.Learns.QuickStart;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/// <summary>
/// This Test class using Syntax API to transform the structure of a C# program.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/get-started/syntax-transformation"/>
public class SyntaxTransformationTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    private const string SampleCode = """
        using System;
        using System.Collections;
        using System.Linq;
        using System.Text;

        namespace HelloWorld
        {
            class Program
            {
                static void Main(string[] args)
                {
                    Console.WriteLine("Hello, World!");
                }
            }
        }
        """;

    [Fact]
    public void When_UsingSyntaxTransformationViaFactoryMethods_Then_UsingStatementIsReplaced()
    {
        NameSyntax name = IdentifierName("System");
        Output?.WriteLine($"\tCreated the identifier {name}");

        Assert.Equal("System", name.ToString());

        name = QualifiedName(name, IdentifierName("Collections"));
        Output?.WriteLine(name.ToString());

        Assert.Equal("System.Collections", name.ToString());

        name = QualifiedName(name, IdentifierName("Generic"));
        Output?.WriteLine(name.ToString());

        Assert.Equal("System.Collections.Generic", name.ToString());

        // Create a modified tree

        SyntaxTree tree = CSharpSyntaxTree.ParseText(SampleCode, cancellationToken: TestContext.Current.CancellationToken);
        var root = (CompilationUnitSyntax)tree.GetRoot(cancellationToken: TestContext.Current.CancellationToken);

        var oldUsing = root.Usings[1];
        var newUsing = oldUsing.WithName(name);
        Output?.WriteLine($"[Before]\n{root}");

        Assert.Equal("using System.Collections;", root.Usings[1].ToString());

        root = root.ReplaceNode(oldUsing, newUsing);
        Output?.WriteLine($"[After]\n{root}");

        Assert.Equal("using System.Collections.Generic;", root.Usings[1].ToString());
    }

    [Fact]
    public void When_UsingSyntaxRewriters_Then_CreatingRefactoring()
    {
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Compilation test = CreateTestCompilation();

        foreach (SyntaxTree sourceTree in test.SyntaxTrees)
        {
            SemanticModel model = test.GetSemanticModel(sourceTree);

            TypeInferenceRewriter rewriter = new TypeInferenceRewriter(model);

            SyntaxNode newSource = rewriter.Visit(sourceTree.GetRoot(cancellationToken: cancellationToken));

            if (newSource != sourceTree.GetRoot(cancellationToken))
            {
                //TODO
                //File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
                Output?.WriteLine($"File {sourceTree.FilePath} has been modified.");
                Output?.WriteLine($"[Before]\n{sourceTree.GetRoot(cancellationToken)}");
                Output?.WriteLine($"[After]\n{newSource}");
            }
        }
    }

#pragma warning disable CA1859 // Use concrete types when possible for improved performance

    private static Compilation CreateTestCompilation()
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        // String programPath = @"..\..\..\Program.cs";
        // String programText = File.ReadAllText(programPath);
        // SyntaxTree programTree =
        //                CSharpSyntaxTree.ParseText(programText)
        //                                .WithFilePath(programPath);

        string rewriterPath = Path.GetFullPath(
            Path.Combine(basePath, @"../../../Learns/QuickStart/TypeInferenceRewriter.cs"));
        string rewriterText = File.ReadAllText(rewriterPath);
        SyntaxTree rewriterTree =
                       CSharpSyntaxTree.ParseText(rewriterText)
                                       .WithFilePath(rewriterPath);

        SyntaxTree[] sourceTrees = { rewriterTree };

        MetadataReference mscorlib =
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        MetadataReference codeAnalysis =
                MetadataReference.CreateFromFile(typeof(SyntaxTree).Assembly.Location);
        MetadataReference csharpCodeAnalysis =
                MetadataReference.CreateFromFile(typeof(CSharpSyntaxTree).Assembly.Location);

        MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis };

        return CSharpCompilation.Create("TransformationCS",
            sourceTrees,
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}
