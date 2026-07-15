using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Examples.Analyzer.Roslyn.Tests.Learns.QuickStart;

/// <summary>
/// This Test class using Syntax API to analyze the structure of a C# program.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis"/>
public class SyntaxAnalysisTests
{
    private static ITestOutputHelper? Output => TestContext.Current.TestOutputHelper;

    [Fact]
    public void When_UsingManualTraversal_Then_ReturnsTraversingTrees()
    {
        const string programText = """
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
                        Console.WriteLine(""Hello, World!"");
                    }
                }
            }
            """;

        SyntaxTree tree = CSharpSyntaxTree.ParseText(programText, cancellationToken: TestContext.Current.CancellationToken);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot(cancellationToken: TestContext.Current.CancellationToken);

        Output?.WriteLine($"The tree is a {root.Kind()} node.");
        Output?.WriteLine($"The tree has {root.Members.Count} elements in it.");
        Output?.WriteLine($"The tree has {root.Usings.Count} using directives. They are:");
        foreach (UsingDirectiveSyntax element in root.Usings)
        {
            Output?.WriteLine($"\t{element.Name}");
        }

        Assert.Equal(SyntaxKind.CompilationUnit, root.Kind());
        Assert.Single(root.Members);
        Assert.Equal(4, root.Usings.Count);

        MemberDeclarationSyntax firstMember = root.Members[0];
        Output?.WriteLine($"The first member is a {firstMember.Kind()}.");
        var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;

        Assert.Equal(SyntaxKind.NamespaceDeclaration, firstMember.Kind());

        Output?.WriteLine($"There are {helloWorldDeclaration.Members.Count} members declared in this namespace.");
        Output?.WriteLine($"The first member is a {helloWorldDeclaration.Members[0].Kind()}.");

        Assert.Single(helloWorldDeclaration.Members);
        Assert.Equal(SyntaxKind.ClassDeclaration, helloWorldDeclaration.Members[0].Kind());

        var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
        Output?.WriteLine($"There are {programDeclaration.Members.Count} members declared in the {programDeclaration.Identifier} class.");
        Output?.WriteLine($"The first member is a {programDeclaration.Members[0].Kind()}.");
        var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];

        Assert.Single(programDeclaration.Members);
        Assert.Equal("Program", programDeclaration.Identifier.Text);
        Assert.Equal(SyntaxKind.MethodDeclaration, programDeclaration.Members[0].Kind());

        Output?.WriteLine($"The return type of the {mainDeclaration.Identifier} method is {mainDeclaration.ReturnType}.");
        Output?.WriteLine($"The method has {mainDeclaration.ParameterList.Parameters.Count} parameters.");
        foreach (ParameterSyntax item in mainDeclaration.ParameterList.Parameters)
        {
            Output?.WriteLine($"The type of the {item.Identifier} parameter is {item.Type}.");
        }
        Output?.WriteLine($"The body text of the {mainDeclaration.Identifier} method follows:");
        Output?.WriteLine(mainDeclaration.Body?.ToFullString() ?? "");

        Assert.Equal("Main", mainDeclaration.Identifier.Text);
        Assert.Equal("void", mainDeclaration.ReturnType.ToString());
        Assert.Single(mainDeclaration.ParameterList.Parameters);

        var argsParameter = mainDeclaration.ParameterList.Parameters[0];

        Assert.Equal("args", argsParameter.Identifier.Text);
        Assert.Equal("string[]", argsParameter.Type?.ToString());

        // Querying the syntax tree using LINQ

        var firstParameters = from methodDeclaration in root.DescendantNodes()
                                        .OfType<MethodDeclarationSyntax>()
                              where methodDeclaration.Identifier.ValueText == "Main"
                              select methodDeclaration.ParameterList.Parameters.First();

        var argsParameter2 = firstParameters.Single();

        Assert.Equal(argsParameter, argsParameter2);
    }

    private class UsingCollector : CSharpSyntaxWalker
    {
        public ICollection<UsingDirectiveSyntax> Usings { get; } = new List<UsingDirectiveSyntax>();

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            Output?.WriteLine($"\tVisitUsingDirective called with {node.Name}.");
            if (node.Name is not null &&
                node.Name.ToString() != "System" &&
                !node.Name.ToString().StartsWith("System."))
            {
                Output?.WriteLine($"\t\tSuccess. Adding {node.Name}.");
                this.Usings.Add(node);
            }
        }
    }

    [Fact]
    public void When_UsingSyntaxWalkers_Then_ReturnsTraversingTreeNode()
    {
        const string programText = """
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using Microsoft.CodeAnalysis;
            using Microsoft.CodeAnalysis.CSharp;

            namespace TopLevel
            {
                using Microsoft;
                using System.ComponentModel;

                namespace Child1
                {
                    using Microsoft.Win32;
                    using System.Runtime.InteropServices;

                    class Foo { }
                }

                namespace Child2
                {
                    using System.CodeDom;
                    using Microsoft.CSharp;

                    class Bar { }
                }
            }
            """;

        SyntaxTree tree = CSharpSyntaxTree.ParseText(programText, cancellationToken: TestContext.Current.CancellationToken);
        CompilationUnitSyntax root = tree.GetCompilationUnitRoot(cancellationToken: TestContext.Current.CancellationToken);

        var collector = new UsingCollector();
        collector.Visit(root);

        foreach (var directive in collector.Usings)
        {
            Output?.WriteLine(directive.Name?.ToString() ?? "");
        }

        Assert.Equal(5, collector.Usings.Count);
    }
}
