using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TripleSix.Core.Helpers
{
    /// <summary>
    /// Helper xử lý Http.
    /// </summary>
    public static class HttpHelper
    {
        private static readonly string[] _excludeHeaderKeys = new[]
        {
           "Accept",
           "Accept-Encoding",
           "Connection",
           "Cache-Control",
           "Content-Length",
           "Host",
           "Postman-Token",
           "User-Agent",
        };

        /// <summary>
        /// Chuyển đổi <see cref="HttpRequest"/> sang curl.
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/> cần xử lý.</param>
        /// <param name="excludeHeaderKeys">Danh sách header keys loại bỏ.</param>
        /// <returns>Chuỗi Curl tương ứng.</returns>
        public static async Task<string> ToCurl(this HttpRequest request, string[]? excludeHeaderKeys = null)
        {
            if (excludeHeaderKeys == null) excludeHeaderKeys = _excludeHeaderKeys;

            var curls = new List<string>
            {
                "curl",
                $"-X {request.Method}",
                $"'{request.Scheme}://{request.Host.Value}{request.Path.Value}{request.QueryString.Value}'",
            };

            foreach (var header in request.Headers)
            {
                if (excludeHeaderKeys.Any(x => x == header.Key)) continue;
                curls.Add($"-H '{header.Key}: {header.Value}'");
            }

            request.EnableBuffering();
            using (var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true))
            {
                var bodyText = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                if (!bodyText.IsNullOrWhiteSpace())
                    curls.Add($"--data '{JObject.Parse(bodyText).ToString(Formatting.None, JsonHelper.Converters)}'");
            }

            return curls.ToString(" ");
        }
    }
}