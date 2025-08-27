using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

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
            excludeHeaderKeys ??= _excludeHeaderKeys;

            var curls = new List<string>
            {
                "curl",
                $"-L -X {request.Method}",
                $"'{request.Scheme}://{request.Host.Value}{request.Path.Value}{request.QueryString.Value}'",
            };

            foreach (var header in request.Headers)
            {
                if (excludeHeaderKeys.Any(x => x == header.Key)) continue;
                curls.Add($"-H '{header.Key}: {header.Value}'");
            }

            if (request.ContentType.IsNotNullOrEmpty() && request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                request.EnableBuffering();
                using var reader = new StreamReader(
                    request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: -1,
                    leaveOpen: true);
                request.Body.Position = 0L;
                string bodyText = await reader.ReadToEndAsync();
                request.Body.Position = 0L;

                if (bodyText.IsNotNullOrEmpty())
                {
                    bodyText = Regex.Unescape(bodyText);
                    curls.Add($"-d '{bodyText}'");
                }
            }

            return curls.ToString(" ");
        }

        /// <summary>
        /// Chuyển đổi <see cref="HttpRequestMessage"/> sang curl.
        /// </summary>
        /// <param name="requestMessage"><see cref="HttpRequestMessage"/> cần xử lý.</param>
        /// <param name="excludeHeaderKeys">Danh sách header keys loại bỏ.</param>
        /// <returns>Chuỗi Curl tương ứng.</returns>
        public static async Task<string> ToCurl(this HttpRequestMessage requestMessage, string[]? excludeHeaderKeys = null)
        {
            if (requestMessage.RequestUri == null) return string.Empty;
            excludeHeaderKeys ??= _excludeHeaderKeys;

            var curls = new List<string>
            {
                "curl",
                $"-L -X {requestMessage.Method}",
                $"'{requestMessage.RequestUri.AbsoluteUri}'",
            };

            foreach (var header in requestMessage.Headers)
            {
                if (excludeHeaderKeys.Any(x => x == header.Key)) continue;
                curls.Add($"-H '{header.Key}: {header.Value.First()}'");
            }

            if (requestMessage.Content != null)
            {
                if (requestMessage.Content.Headers.ContentType != null && requestMessage.Content.Headers.ContentType.MediaType == "application/json")
                {
                    var bodyText = await requestMessage.Content.ReadAsStringAsync();
                    if (bodyText.IsNotNullOrEmpty())
                    {
                        curls.Add($"-H 'Content-Type: application/json'");
                        curls.Add($"-d '{bodyText}'");
                    }
                }
            }

            return curls.ToString(" ");
        }

        /// <summary>
        /// Lấy claim từ danh sách theo name.
        /// </summary>
        /// <param name="claims">Danh sách claim.</param>
        /// <param name="name">Name của claim cần lấy.</param>
        /// <returns><see cref="Claim"/>.</returns>
        public static Claim? GetClaim(this IEnumerable<Claim> claims, string name)
        {
            return claims.FirstOrDefault(x => x.Type.ToLower() == nameof(name).ToLower());
        }
    }
}