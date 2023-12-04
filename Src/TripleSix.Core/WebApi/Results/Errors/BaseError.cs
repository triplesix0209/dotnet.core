using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.WebApi.Results
{
    public class BaseError
        : IError
    {
        private string _code;

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
    }
}