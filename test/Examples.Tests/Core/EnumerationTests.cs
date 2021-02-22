using ChainingAssertion;
using Xunit;

#pragma warning disable IDE0051

namespace Examples.Core
{
    public class MeasureValueTests
    {
        private class CardType : Enumeration
        {
            public static readonly CardType Amex = new(1, nameof(Amex));
            public static readonly CardType Visa = new(2, nameof(Visa));
            public static readonly CardType MasterCard = new(3, nameof(MasterCard));

            public CardType(int id, string name)
                : base(id, name)
            {
            }
        }

        [Fact]
        void TestEquals()
        {
            var card = new CardType(1, "AMEX");

            (card == CardType.Amex).Is(true);

            // Not work.
            // switch (card)
            // {
            //     case CardType.Amex:
            //         break;
            //     case CardType.Visa:
            //         break;
            //     case CardType.MasterCard:
            //         break;
            //     default:
            //         break;
            // }

            return;
        }

    }

}
