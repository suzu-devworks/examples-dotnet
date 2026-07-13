using System.ComponentModel;

namespace Examples.CodeGenerator.Roslyn.Recipes.EnumDescriptionGenerators;

/// <summary>
/// This is a test enum used for testing the EnumDescriptionGenerator.
/// </summary>
[GenerateDescription]
public enum TestEnum
{
    [Description("This is the first value")]
    FirstValue,

    /// <summary> This is the second value, which has a description attribute. </summary>
    [Description("This is the second value")]
    SecondValue,

    /// <summary>
    /// This is the third value,
    /// which does not have a description attribute.
    /// </summary>
    ThirdValue,

    FourthValue // No description attribute
}
