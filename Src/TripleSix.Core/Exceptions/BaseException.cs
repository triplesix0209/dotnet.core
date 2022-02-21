#pragma warning disable SA1201 // ElementsMustAppearInTheCorrectOrder

using System;
using TripleSix.Core.Attributes;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Exceptions
{
    public class BaseException : Exception,
        IException
    {
        public BaseException(
            int httpCode = 500,
            string code = "exception",
            string message = "unexpected exception",
            object detail = null)
            : base(message)
        {
            HttpCode = httpCode;
            Code = code.ToSnakeCase();
            Detail = detail;
        }

        public BaseException(
            Enum error,
            object detail = null,
            params object[] args)
            : base(GetErrorMessage(error, args))
        {
            var info = EnumHelper.GetErrorData(error.GetType(), error);
            HttpCode = info?.HttpCode ?? 500;
            Code = info?.Code ?? EnumHelper.GetName(error.GetType(), error).ToSnakeCase();
            Detail = detail;
        }

        public string Code { get; }

        public int HttpCode { get; }

        public object Detail { get; set; }

        private static string GetErrorMessage(Enum error, object[] args)
        {
            var info = EnumHelper.GetErrorData(error.GetType(), error);

            return info.Message == null
                ? "unexpected exception"
                : string.Format(info.Message, args.Length == 0 ? info.DefaultArgs : args);
        }
    }

    public enum BaseExceptions
    {
        [ErrorData(404, message: "không tìm thấy {0}", defaultArgs: "đối tượng")]
        ObjectNotFound,

        [ErrorData(400, message: "{0} bị trùng các mã code sau: {1}")]
        CodeDuplicated,

        [ErrorData(400, message: "thông tin đầu vào bị thiếu hoặc không đúng")]
        BadClientRequest,

        [ErrorData(400, message: "lỗi kế thừa lặp")]
        CyclicInheritance,

        [ErrorData(400, message: "những cột sau không phù hợp để sắp xếp: {0}")]
        SortColumnInvalid,
    }
}
