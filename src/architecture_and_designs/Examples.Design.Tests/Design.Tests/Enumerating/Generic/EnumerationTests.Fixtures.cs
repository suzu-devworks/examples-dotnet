using System.Text.RegularExpressions;
using Examples.Design.Enumerating.Generic;

namespace Examples.Design.Tests.Enumerating.Generic;

public partial class EnumerationTests
{
    public sealed class CardType : Enumeration<CardType>
    {
        public static readonly CardType Amex = new("1", nameof(Amex), @"^(?:[1]|[Aa]mex)$");
        public static readonly CardType Visa = new("2", nameof(Visa), @"^(?:[2]|[Vv]isa)$");
        public static readonly CardType MasterCard = new("3", nameof(MasterCard), @"^(?:[3]|[Mm]aster[Cc]ard)$");

        private readonly Regex _pattern;

        private CardType(string value, string display, string? pattern)
            : base(value, display)
        {
            var expression = (pattern is null) ? display : pattern;
            _pattern = new(expression, RegexOptions.Compiled);
        }

        public override bool IsMatch(string value)
            => _pattern.IsMatch(value);

    }

}
