using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    public class BaseMeta
    {
        public BaseMeta()
        {
            TraceId = Activity.Current?.RootId;
        }

        [JsonProperty(Order = -10)]
        [DisplayName("Trace Id")]
        public virtual string? TraceId { get; set; }

        [JsonProperty(Order = -10)]
        [DisplayName("phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; }
    }
}
