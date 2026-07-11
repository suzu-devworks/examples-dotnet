using System.Text.RegularExpressions;

namespace Examples.Text.Tests.RegularExpressions;

/// <summary>
/// Tests to replace with MatchEvaluator.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/api/system.text.regularexpressions.matchevaluator" />
public class ReplaceWithMatchEvaluatorTests
{
    [Fact]
    public void When_SpecifiedCharacters_Then_ReplacesWithSequentialNumbers()
    {
        // replace 'cc' to number of appearances.
        // spell-checker: disable
        var input___ = "aabbccddeeffcccgghhcccciijjcccckkcc";
        //                  VV      VV     VVVV    VVVV  VV
        var expected = "aabb1_ddeeff2_cgghh3_4_iijj5_6_kk7_";
        // spell-checker: enable

        var pattern = @"cc";
        var re = new Regex(pattern);
        var i = 0;

        var actual = re.Replace(input___, m =>
        {
            i++;
            return $"{i}".PadRight(2, '_')[..2];
        });

        Assert.Equal(expected, actual);
    }

}
