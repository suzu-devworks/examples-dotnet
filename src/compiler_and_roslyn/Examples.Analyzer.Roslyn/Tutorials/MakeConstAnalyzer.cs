using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Examples.Analyzer.Roslyn.Tutorials;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MakeConstAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MakeConst";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
        // Configure generated code analysis (RS1025)
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        // Enable concurrency (RS1026)
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        // find local declarations. Add
        var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

        // make sure the declaration isn't already const:
        if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
        {
            return;
        }

        TypeSyntax variableTypeName = localDeclaration.Declaration.Type;
        ITypeSymbol? variableType = context.SemanticModel.GetTypeInfo(variableTypeName, context.CancellationToken).ConvertedType;

        if (variableType is null)
        {
            return;
        }

        // Ensure that all variables in the local declaration have initializers that
        // are assigned with constant values.
        foreach (VariableDeclaratorSyntax variable in localDeclaration.Declaration.Variables)
        {
            EqualsValueClauseSyntax? initializer = variable.Initializer;
            if (initializer is null)
            {
                return;
            }

            Optional<object?> constantValue = context.SemanticModel.GetConstantValue(initializer.Value, context.CancellationToken);
            if (!constantValue.HasValue)
            {
                return;
            }

            // Ensure that the initializer value can be converted to the type of the
            // local declaration without a user-defined conversion.
            Conversion conversion = context.SemanticModel.ClassifyConversion(initializer.Value, variableType);
            if (!conversion.Exists || conversion.IsUserDefined)
            {
                return;
            }

            // Special cases:
            //  * If the constant value is a string, the type of the local declaration
            //    must be System.String.
            //  * If the constant value is null, the type of the local declaration must
            //    be a reference type.
            if (constantValue.Value is string)
            {
                if (variableType.SpecialType != SpecialType.System_String)
                {
                    return;
                }
            }
            else if (variableType.IsReferenceType && constantValue.Value != null)
            {
                return;
            }
        }

        // Perform data flow analysis on the local declaration.
        DataFlowAnalysis? dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);

        foreach (VariableDeclaratorSyntax variable in localDeclaration.Declaration.Variables)
        {
            // Retrieve the local symbol for each variable in the local declaration
            // and ensure that it is not written outside of the data flow analysis region.
            ISymbol? variableSymbol = context.SemanticModel.GetDeclaredSymbol(variable, context.CancellationToken);
            if (variableSymbol is not null &&
                dataFlowAnalysis is not null &&
                dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                return;
            }
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
    }
}
