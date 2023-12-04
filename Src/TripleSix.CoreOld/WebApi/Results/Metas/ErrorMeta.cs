using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.CoreOld.WebApi.Results
{
    public class ErrorMeta : IMeta
    {
        [JsonProperty(Order = -10)]
        [DisplayName("phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; } = false;
    }
}