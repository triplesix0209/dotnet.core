using System.Security.Claims;
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
                request.Body.Position = 0;
                var bodyText = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                if (bodyText.IsNotNullOrEmpty())
                {
                    try
                    {
                        curls.Add($"--data '{JObject.Parse(bodyText).ToString(Formatting.None, JsonHelper.Converters)}'");
                    }
                    catch
                    {
                        curls.Add($"--data '{bodyText}'");
                    }
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
            if (excludeHeaderKeys == null) excludeHeaderKeys = _excludeHeaderKeys;

            var curls = new List<string>
            {
                "curl",
                $"-X {requestMessage.Method}",
                $"'{requestMessage.RequestUri.Scheme}://{requestMessage.RequestUri.PathAndQuery}'",
            };

            foreach (var header in requestMessage.Headers)
            {
                if (excludeHeaderKeys.Any(x => x == header.Key)) continue;
                curls.Add($"-H '{header.Key}: {header.Value}'");
            }

            if (requestMessage.Content != null)
            {
                var bodyText = await requestMessage.Content.ReadAsStringAsync();
                if (bodyText.IsNotNullOrEmpty())
                {
                    try
                    {
                        curls.Add($"--data '{JObject.Parse(bodyText).ToString(Formatting.None, JsonHelper.Converters)}'");
                    }
                    catch
                    {
                        curls.Add($"--data '{bodyText}'");
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