using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;

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
        [JsonPropertyOrder(-10)]
        [DisplayName("Trace Id")]
        public virtual string? TraceId { get; set; }

        /// <summary>
        /// Phiên xử lý thành công hay thất bại?.
        /// </summary>
        [JsonPropertyOrder(-10)]
        [DisplayName("Phiên xử lý thành công hay thất bại?")]
        public virtual bool Success { get; set; }
    }
}
