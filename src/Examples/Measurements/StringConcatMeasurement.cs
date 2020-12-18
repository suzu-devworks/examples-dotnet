using System.Linq;
using System.Text;
using Examples.Utilities;

namespace Examples.Measurements
{
    [Exclude]
    class StringConcatMeasurement : IRunner
    {
        public void Run()
        {
            var each = Enumerable.Range(0, 30000).Select(x => x.ToString());

            var result1 = FunctionMeasurer.As("operator +").Run(() => each.Aggregate((a, b) => a + "," + b));

            var result2 = FunctionMeasurer.As("string.Concat(\",\")").Run(() => each.Aggregate((a, b) => string.Concat(a, ",", b)));

            var result3 = FunctionMeasurer.As("string.Concat(\',\')").Run(() => each.Aggregate((a, b) => string.Concat(a, ',', b)));

            var result4 = FunctionMeasurer.As("StringBuilder.Append()").Run(() =>
            {
                var aggregates = new StringBuilder();
                _ = each.Aggregate((a, b) =>
                {
                    if (aggregates.Length > 0) { aggregates.Append(','); }
                    aggregates.Append(b);
                    return null;
                });
                return aggregates.ToString();
            });

            var result5 = FunctionMeasurer.As("string.Join(\',\')").Run(() => string.Join(",", each));

            return;
        }

    }
}
