using RestSharp;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Exceptions
{
    /// <summary>
    /// Lỗi gọi API thất bại.
    /// </summary>
    public class RequestFailedException : BaseException
    {
        /// <summary>
        /// Khởi tạo <see cref="RequestFailedException"/>.
        /// </summary>
        /// <param name="response"><see cref="RestResponse"/>.</param>
        /// <param name="message">Thông báo lỗi tùy chọn.</param>
        public RequestFailedException(RestResponse response, string? message = null)
            : base(FormatMessage(response, message))
        {
            Data = new Dictionary<string, object>
            {
                { "ApiPath", response.Request?.Resource ?? string.Empty },
            };
        }

        private static string FormatMessage(RestResponse response, string? message)
        {
            if (message.IsNotNullOrEmpty())
                return $"Lỗi API {message}";

            var errorMessage = response.Content ?? response.ErrorMessage ?? response.ErrorException?.Message;
            errorMessage = errorMessage?.Trim();

            return $"Lỗi API {errorMessage}";
        }
    }
}
