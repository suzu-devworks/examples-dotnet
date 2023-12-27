using System.Text.RegularExpressions;

namespace Examples.Text.Tests._.System.Text.RegularExpressions;

/// <summary>
/// Tests to replace with MatchEvaluator.
/// </summary>
/// <seealso href="https://docs.microsoft.com/ja-jp/dotnet/api/system.text.regularexpressions.matchevaluator" />
public class ReplaceWithMatchEvaluatorTests
{
    [Fact]
    public void WhenCallingReplaceWithMatchEvaluator()
    {
        // replace 'cc' to number of appearances.
        var input___ = "aabbccddeeffcccgghhcccciijjcccckkcc";
        //                  VV      VV     VVVV    VVVV  VV
        var expected = "aabb1_ddeeff2_cgghh3_4_iijj5_6_kk7_";

        var pattern = @"cc";
        var re = new Regex(pattern);
        var i = 0;

        re.Replace(input___, m =>
        {
            i++;
            return $"{i}".PadRight(2, '_')[..2];
        })
        .Is(expected);

    }

}
