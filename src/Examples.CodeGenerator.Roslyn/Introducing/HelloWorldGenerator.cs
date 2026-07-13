using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Examples.CodeGenerator.Roslyn.Introducing;

[Generator]
public class HelloWorldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // begin creating the source we'll inject into the users compilation

        // **There's a performance issue where even a single character change in any one file
        //  within the project causes the entire SyntaxTree to be re-evaluated
        //  (caching completely discarded)**.
        // IncrementalValueProvider<IEnumerable<SyntaxTree>> syntaxTreesProvider =
        //             context.CompilationProvider.Select((compilation, cancellationToken) => compilation.SyntaxTrees);

        IncrementalValuesProvider<SyntaxTree> syntaxTreesProvider = context.SyntaxProvider
                    .CreateSyntaxProvider(
                        // 1. Filter (true to pass everything, check here to narrow down to specific attributes or types)
                        predicate: static (node, cancellationToken) => node is Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax,
                        // 2. Transformation (get the SyntaxTree to which it belongs from the node)）
                        transform: static (syntaxContext, cancellationToken) => syntaxContext.Node.SyntaxTree
                    )
                    // Eliminate duplication (multiple nodes in the same file) and make it into a SyntaxTree unit
                    .WithComparer(EqualityComparer<SyntaxTree>.Default);

        // The cache and reconstructed into a single array at lightning speed by .Collect().
        IncrementalValueProvider<ImmutableArray<SyntaxTree>> collectedTreesProvider = syntaxTreesProvider.Collect();

        context.RegisterSourceOutput(collectedTreesProvider, static (ctx, syntaxTree) =>
        {
            // begin creating the source we'll inject into the users compilation
            var sourceBuilder = new StringBuilder();

            // add the filepath of each tree to the class we're building
            foreach (SyntaxTree tree in syntaxTree)
            {
                sourceBuilder.AppendLine($@"            writer.WriteLine(@"" - {tree.FilePath}"");");
            }

            // finish creating the source to inject
            var source = $$"""
                using System;
                namespace HelloWorldGenerated
                {
                    public static class HelloWorld
                    {
                        public static void SayHello(TextWriter writer)
                        {
                            writer.WriteLine("Hello from generated code!");
                            writer.WriteLine("The following syntax trees existed in the compilation that created this program:");
                {{sourceBuilder}}
                        }
                    }
                }
                """;

            ctx.AddSource("HelloWorldGenerator.g", SourceText.From(source, Encoding.UTF8));
        });
    }
}
