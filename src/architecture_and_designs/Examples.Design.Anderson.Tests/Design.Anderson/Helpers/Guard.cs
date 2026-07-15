using Examples.Design.Anderson.Exceptions;

namespace Examples.Design.Anderson.Helpers
{
    public static class Guard
    {
        public static void IsNullOrEmptyMessage(string? value, string message)
        {
            IsNullOrEmptyMessage(value, message, ExceptionType.Information);
        }

        public static void IsNullOrEmptyMessage(string? value, string message, ExceptionType type)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new MessageException(type, message);
            }
        }

    }

}
