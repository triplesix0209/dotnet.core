using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi
{
    public class ErrorInfo
    {
        private string _code;

        public ErrorInfo(string code, string message, object? data)
        {
            _code = code.ToSnakeCase();
            Message = message;
            Data = data;
        }

        [JsonProperty(Order = -10)]
        [DisplayName("mã lỗi")]
        public virtual string Code
        {
            get => _code;
            set => _code = value.ToSnakeCase();
        }

        [JsonProperty(Order = -10)]
        [DisplayName("mô tả lỗi")]
        public virtual string Message { get; set; }

        [JsonProperty(Order = -10)]
        [DisplayName("Dữ liệu lỗi")]
        public virtual object? Data { get; set; }
    }
}
