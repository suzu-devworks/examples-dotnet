using System;

namespace Examples.Design.Anderson.Helpers
{
    public static class Extensions
    {
        public static float ToSingle(this double value)
        {
            return Convert.ToSingle(value);
        }

        public static DateTime ToDate(this string value)
        {
            return DateTime.Parse(value);
        }
    }

}
