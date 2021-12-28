using System.ComponentModel;
using Newtonsoft.Json;

namespace CpTech.Core.WebApi.Results
{
    public class BaseMeta : IMeta
    {
        [JsonProperty(Order = -10)]
        [DisplayName("phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; }
    }
}