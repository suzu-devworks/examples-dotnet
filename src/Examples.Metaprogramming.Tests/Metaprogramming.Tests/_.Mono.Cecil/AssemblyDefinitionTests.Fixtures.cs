namespace Examples.Metaprogramming.Tests._.Mono.Cecil
{
    namespace Sample
    {
        public class MyDynamicType
        {
            private int m_number;

            public MyDynamicType() : this(42) { }
            public MyDynamicType(int initNumber)
            {
                m_number = initNumber;
            }

            public int Number
            {
                get { return m_number; }
                set { m_number = value; }
            }

            public int MyMethod(int multiplier)
            {
                return m_number * multiplier;
            }
        }


        public static class MyDynamicExtensions
        {
            public static string DoExtension(this MyDynamicType source, string appended)
            {
                var number = source.Number.ToString();
                var value = string.Concat(number, appended);
                return value;
            }
        }

        public class MyDynamicOpenGeneric<T>
        {
            private readonly T? _value;

            public MyDynamicOpenGeneric(T value)
            {
                _value = value;
            }

            public T? Get() => _value;

            public (T?, T?) DoMethod<T1, T2>(T1? param1, T2? param2)
                where T1 : T
                where T2 : T
            {
                return (param1, param2);
            }

        }

    }

}
