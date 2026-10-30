using System;
using Examples.Design.Enumerating.JavaStyle;

namespace Examples.Design.Tests.Enumerating.JavaStyle;

public partial class EnumerationTests
{
    public class CardType : Enumeration<CardType>
    {
        public static readonly CardType Amex = new(1, nameof(Amex), () => nameof(Amex));
        public static readonly CardType Visa = new(2, nameof(Visa), () => nameof(Visa));
        public static readonly CardType MasterCard = new(3, nameof(MasterCard), () => nameof(MasterCard));

        public int Id { get; init; }
        private readonly Func<string> _provider;
        private static int _counter = 0;

        public string GetProvider() => $"this card is {_provider()}.";

        private CardType(int id, string name, Func<string> provider)
            : base(name, _counter++)
        {
            Id = id;
            _provider = provider;
        }

    }
}
