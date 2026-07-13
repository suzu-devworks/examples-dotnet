using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Examples.Analyzer.Roslyn.Tests.Learns.QuickStart;

/// <summary>
/// This Test class using Semantic API to analyze the meaning of a C# program.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/get-started/semantic-analysis"/>
public class SemanticAnalysisTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    private const string ProgramText = """
            using System;
            using System.Collections.Generic;
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
    public void When_UsingQueryingSymbolsByBindingAName_Then_ReturnsSymbolInformation()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(ProgramText, cancellationToken: TestContext.Current.CancellationToken);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot(cancellationToken: TestContext.Current.CancellationToken);

        var compilation = CSharpCompilation.Create("HelloWorld")
            .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
            .AddSyntaxTrees(tree);

        // A SemanticModel can answer questions like
        //  "What names are in scope at this location?",
        //  "What members are accessible from this method?",
        //  "What variables are used in this block of text?",
        //   and
        //  "What does this name/expression refer to?"
        SemanticModel model = compilation.GetSemanticModel(tree);

        Assert.NotNull(model);

        // Use the syntax tree to find "using System;"
        UsingDirectiveSyntax usingSystem = root.Usings[0];
        NameSyntax systemName = usingSystem.Name!;

        Assert.Equal("System", systemName.ToString());
        Assert.Equal("System", usingSystem.Name?.ToString());

        // Use the semantic model for symbol information:
        SymbolInfo nameInfo = model.GetSymbolInfo(systemName, cancellationToken: TestContext.Current.CancellationToken);

        var systemSymbol = (INamespaceSymbol?)nameInfo.Symbol;
        if (systemSymbol?.GetNamespaceMembers() is not null)
        {
            foreach (INamespaceSymbol ns in systemSymbol?.GetNamespaceMembers()!)
            {
                Output?.WriteLine(ns?.ToString() ?? "");
            }
        }

        Assert.Equal("System", systemSymbol?.ToString());
        Assert.NotEmpty(systemSymbol?.GetNamespaceMembers() ?? []);
    }

    [Fact]
    public void When_UsingQueryingSymbolsByBindingAnExpression_Then_ReturnsSymbolInformation()
    {
        SyntaxTree tree = CSharpSyntaxTree.ParseText(ProgramText, cancellationToken: TestContext.Current.CancellationToken);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot(cancellationToken: TestContext.Current.CancellationToken);

        var compilation = CSharpCompilation.Create("HelloWorld")
            .AddReferences(MetadataReference.CreateFromFile(typeof(string).Assembly.Location))
            .AddSyntaxTrees(tree);

        SemanticModel model = compilation.GetSemanticModel(tree);

        // Use the syntax model to find the literal string:
        LiteralExpressionSyntax helloWorldString = root.DescendantNodes()
            .OfType<LiteralExpressionSyntax>()
            .Single();

        Assert.Equal("\"Hello, World!\"", helloWorldString.ToString());

        // Use the semantic model for type information:
        TypeInfo literalInfo = model.GetTypeInfo(helloWorldString, cancellationToken: TestContext.Current.CancellationToken);

        var stringTypeSymbol = (INamedTypeSymbol?)literalInfo.Type;

        var format = new SymbolDisplayFormat(
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
        );
        Assert.Equal("System.String", stringTypeSymbol?.ToDisplayString(format));
        Assert.Equal("string", stringTypeSymbol?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

        var allMembers = stringTypeSymbol?.GetMembers();

        Assert.NotEmpty(allMembers ?? []);

        var methods = allMembers?.OfType<IMethodSymbol>();

        Assert.NotEmpty(methods ?? []);

        var publicStringReturningMethods = methods?
            .Where(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, stringTypeSymbol) &&
            m.DeclaredAccessibility == Accessibility.Public);

        Assert.NotEmpty(publicStringReturningMethods ?? []);

        var distinctMethods = publicStringReturningMethods?
            .OrderBy(m => m.Name).Select(m => m.Name).Distinct();

        Assert.NotEmpty(distinctMethods ?? []);

        var distinctMethods2 = (from method in stringTypeSymbol?.GetMembers().OfType<IMethodSymbol>()
                                where SymbolEqualityComparer.Default.Equals(method.ReturnType, stringTypeSymbol) &&
                                   method.DeclaredAccessibility == Accessibility.Public
                                orderby method.Name
                                select method.Name).Distinct();

        foreach (string name in distinctMethods2 ?? [])
        {
            Output?.WriteLine(name);
        }

        Assert.Equal(distinctMethods, distinctMethods2);
    }
}
