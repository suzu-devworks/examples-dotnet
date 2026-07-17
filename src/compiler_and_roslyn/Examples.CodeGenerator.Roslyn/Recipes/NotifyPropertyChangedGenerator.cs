using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable RS2008 // Enable analyzer release tracking

namespace Examples.CodeGenerator.Roslyn.Recipes;

[Generator]
public class NotifyPropertyChangedGenerator : IIncrementalGenerator
{
    private const string AttributeSource = """
        using System;

        namespace Examples.CodeGenerator.Generated
        {
            [global::Microsoft.CodeAnalysis.Embedded]
            [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
            internal sealed class AutoNotifyAttribute : Attribute
            {
                public AutoNotifyAttribute()
                {
                }

                public string PropertyName { get; set; }
            }
        }
        """;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            postInitializationContext.AddEmbeddedAttributeDefinition();
            postInitializationContext.AddSource("AutoNotifyAttribute.g.cs",
                SourceText.From(AttributeSource, Encoding.UTF8));
        });

        IncrementalValuesProvider<IFieldSymbol> fields = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "Examples.CodeGenerator.Generated.AutoNotifyAttribute",
                predicate: static (_, _) => true,
                transform: static (context, _) => (IFieldSymbol)context.TargetSymbol);

        IncrementalValueProvider<(Compilation Left, ImmutableArray<IFieldSymbol> Right)> compilationAndFields =
            context.CompilationProvider.Combine(fields.Collect());

        context.RegisterSourceOutput(compilationAndFields,
                 static (ctx, source) => Execute(source.Left, source.Right, ctx));
    }

    private static void Execute(Compilation compilation, ImmutableArray<IFieldSymbol> fields, SourceProductionContext context)
    {

        if (fields.IsEmpty)
        {
            return;
        }

        // Get marker attribute type symbol
        INamedTypeSymbol? attributeSymbol = compilation.GetTypeByMetadataName(
                "Examples.CodeGenerator.Generated.AutoNotifyAttribute");
        if (attributeSymbol is null) { return; }

        // Get INotifyPropertyChanged symbol
        INamedTypeSymbol? notifySymbol = compilation.GetTypeByMetadataName(
                "System.ComponentModel.INotifyPropertyChanged");
        if (notifySymbol is null) { return; }

        // Group the fields by class, and generate the source
        var grouping = fields.GroupBy<IFieldSymbol, INamedTypeSymbol>(f => f.ContainingType, SymbolEqualityComparer.Default);
        foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in grouping)
        {
            var classSource = ProcessClass(group.Key, group.AsEnumerable(), attributeSymbol, notifySymbol, context);
            if (classSource is not null)
            {
                context.AddSource($"{group.Key.Name}_autoNotify.g.cs", SourceText.From(classSource!, Encoding.UTF8));
            }
        }
    }

    private static readonly DiagnosticDescriptor NestedClassNotSupportedDiagnostic = new DiagnosticDescriptor(
        id: "AUTOGEN001",
        title: "Nested class not supported",
        messageFormat: "Class '{0}' is nested inside another type. AutoNotify is only supported for top-level classes.",
        category: "NotifyPropertyChangedGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor InvalidPropertyNameDiagnostic = new DiagnosticDescriptor(
        id: "AUTOGEN002",
        title: "Invalid property name",
        messageFormat: "Field '{0}' cannot be processed. The property name is either empty or identical to the field name.",
        category: "NotifyPropertyChangedGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    private static string? ProcessClass(INamedTypeSymbol classSymbol, IEnumerable<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, SourceProductionContext context)
    {
        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
        {
            context.ReportDiagnostic(Diagnostic.Create(
                NestedClassNotSupportedDiagnostic,
                classSymbol.Locations.FirstOrDefault(),
                classSymbol.Name));
            return null;
        }

        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

        StringBuilder source = new();
        source.AppendLine($$"""
            namespace {{namespaceName}}
            {
                public partial class {{classSymbol.Name}} : {{notifySymbol.ToDisplayString()}}
                {
            """);

        // If the class doesn't implement INotifyPropertyChanged already, add it
        if (!classSymbol.Interfaces.Contains(notifySymbol, SymbolEqualityComparer.Default))
        {
            source.AppendLine("""
                    public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            """);
        }

        // Create properties for each field
        foreach (IFieldSymbol fieldSymbol in fields)
        {
            ProcessField(source, fieldSymbol, attributeSymbol, context);
        }

        source.AppendLine("""
                }
            }
            """);

        return source.ToString();
    }

    private static void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol, SourceProductionContext context)
    {
        // Get the name and type of the field
        string fieldName = fieldSymbol.Name;
        ITypeSymbol fieldType = fieldSymbol.Type;

        // Get the AutoNotify attribute from the field, and any associated data
        AttributeData attributeData = fieldSymbol.GetAttributes()
            .Single(ad => ad.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) ?? false);
        TypedConstant overriddenNameOpt = attributeData.NamedArguments
            .SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

        string propertyName = ChooseName(fieldName, overriddenNameOpt);
        if (propertyName.Length == 0 || propertyName == fieldName)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                InvalidPropertyNameDiagnostic,
                fieldSymbol.Locations.FirstOrDefault(),
                fieldSymbol.Name));
            return;
        }

        source.AppendLine($$"""
                    public {{fieldType}} {{propertyName}}
                    {
                        get
                        {
                            return this.{{fieldName}};
                        }
                        set
                        {
                            this.{{fieldName}} = value;
                            this.PropertyChanged?.Invoke(this, new global::System.ComponentModel.PropertyChangedEventArgs(nameof({{propertyName}})));
                        }
                    }
            """);

        static string ChooseName(string fieldName, TypedConstant overriddenNameOpt)
        {
            if (!overriddenNameOpt.IsNull)
            {
                return overriddenNameOpt.Value!.ToString();
            }

            fieldName = fieldName.TrimStart('_');
            if (fieldName.Length == 0)
            {
                return string.Empty;
            }

            if (fieldName.Length == 1)
            {
                return fieldName.ToUpper();
            }

            return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
        }
    }
}
