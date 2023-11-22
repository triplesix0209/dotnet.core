using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Error info.
    /// </summary>
    public class ErrorInfo
    {
        private string _code;

        /// <summary>
        /// Error info.
        /// </summary>
        /// <param name="code">Mã lỗi.</param>
        /// <param name="message">Mô tả lỗi.</param>
        /// <param name="data">Dữ liệu lỗi.</param>
        public ErrorInfo(string code, string message, object? data)
        {
            _code = code.ToSnakeCase();
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Mã lỗi.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Mã lỗi")]
        public virtual string Code
        {
            get => _code;
            set => _code = value.ToSnakeCase();
        }

        /// <summary>
        /// Mô tả lỗi.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Mô tả lỗi")]
        public virtual string Message { get; set; }

        /// <summary>
        /// Dữ liệu lỗi.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Dữ liệu lỗi")]
        public virtual object? Data { get; set; }
    }
}
