using System;
using TripleSix.CoreOld.Helpers;

namespace TripleSix.CoreOld.Attributes
{
    public class ErrorDataAttribute : Attribute
    {
        public ErrorDataAttribute(
            int httpCode = 500,
            string code = null,
            string message = null,
            params object[] defaultArgs)
        {
            HttpCode = httpCode;
            Code = code?.ToSnakeCase();
            Message = message;
            DefaultArgs = defaultArgs;
        }

        public string Code { get; protected set; }

        public int HttpCode { get; protected set; }

        public string Message { get; protected set; }

        public object[] DefaultArgs { get; protected set; }
    }
}