using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public class BaseMeta
    {
        [JsonProperty(Order = -10)]
        [DisplayName("phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; }
    }
}
