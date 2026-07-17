using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Examples.Analyzer.Roslyn.Tests.Learns.QuickStart;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

public class TypeInferenceRewriter(SemanticModel semanticModel) : CSharpSyntaxRewriter
{
    private readonly SemanticModel _semanticModel = semanticModel;

    public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
        if (node.Declaration.Variables.Count > 1)
        {
            return node;
        }

        if (node.Declaration.Variables[0].Initializer is null)
        {
            return node;
        }

        VariableDeclaratorSyntax declarator = node.Declaration.Variables.First();
        TypeSyntax variableTypeName = node.Declaration.Type;

        ITypeSymbol? variableType = (ITypeSymbol?)_semanticModel
            .GetSymbolInfo(variableTypeName)
            .Symbol;

        TypeInfo initializerInfo = _semanticModel.GetTypeInfo(declarator.Initializer!.Value);

        if (SymbolEqualityComparer.Default.Equals(variableType, initializerInfo.Type))
        {
            TypeSyntax varTypeName = IdentifierName("var")
                .WithLeadingTrivia(variableTypeName.GetLeadingTrivia())
                .WithTrailingTrivia(variableTypeName.GetTrailingTrivia());

            return node.ReplaceNode(variableTypeName, varTypeName);
        }
        else
        {
            return node;
        }
    }
}
