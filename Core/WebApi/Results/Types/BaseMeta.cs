using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Base meta.
    /// </summary>
    public class BaseMeta
    {
        /// <summary>
        /// Base meta.
        /// </summary>
        public BaseMeta()
        {
            TraceId = Activity.Current?.RootId;
        }

        /// <summary>
        /// Trace Id.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Trace Id")]
        public virtual string? TraceId { get; set; }

        /// <summary>
        /// Phiên xử lý thành công hay thất bại?.
        /// </summary>
        [JsonProperty(Order = -10)]
        [DisplayName("Phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; }
    }
}
