using Xunit;

#pragma warning disable IDE0051

namespace Examples.Features.CS9
{
    public class InitOnlySetterTests
    {
        private class OldStyleClass
        {
            public string Code { get; private set; }
            public string Name { get; private set; }
        }

        private class AutoPropertyClass
        {
            public string Code { get; init; }
            public string Name { get; init; }
        }

        private class ManualPropertyClass
        {
            private string _Code;
            public string Code
            {
                get { return _Code; }
                init { _Code = value; }
            }

            private string _Name;
            public string Name
            {
                get { return _Name; }
                init { _Name = value; }
            }
        }

        [Fact]
        void Test()
        {
            _ = new OldStyleClass();
            //#CS0272 o.Code = "123";
            //#CS0272 o = new OldStyleClass() { Code = "123", Name = "HOGE" };

            _ = new AutoPropertyClass();
            //#CS8852 a.Code = "123";
            _ = new AutoPropertyClass() { Code = "123", Name = "HOGE" };

            _ = new ManualPropertyClass();
            //#CS8852 m.Code = "123";
            _ = new ManualPropertyClass() { Code = "123", Name = "HOGE" };

            return;
        }
    }
}
