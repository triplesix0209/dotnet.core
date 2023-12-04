using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi.Results
{
    public class SuccessMeta : IMeta
    {
        [JsonProperty(Order = -10)]
        [DisplayName("phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; } = true;
    }
}