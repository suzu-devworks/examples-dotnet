using System;

namespace Examples.Design.Anderson.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase(ExceptionType type, string message) : base(message)
        {
            this.ExceptionType = type;
        }

        public ExceptionType ExceptionType { get; }

    }

}
