using ChainingAssertion;
using Xunit;
using Examples.DDD.Anderson.Domains;

#pragma warning disable IDE0051

namespace Examples.DDD.Anderson
{
    public class MeasureValueTests
    {
        [Fact]
        void TestEquals()
        {
            var object1 = new MeasureValue(1.23456f);

            object1.Equals(new MeasureValue(1.23456f)).Is(true);
            (object1 == new MeasureValue(1.23456f)).Is(true);
            object1.Equals(null).Is(false);
            (object1 == null).Is(false);

            return;
        }

        [Fact]
        void TestNotEquals()
        {
            var object1 = new MeasureValue(1.23456f);

            (object1 != new MeasureValue(1.2345601f)).Is(true);
            (object1 != null).Is(true);

            return;
        }
    }
}
