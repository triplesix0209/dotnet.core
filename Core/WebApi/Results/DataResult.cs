using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Data result.
    /// </summary>
    /// <typeparam name="TData">Loại dữ liệu.</typeparam>
    public class DataResult<TData> : BaseResult<BaseMeta>
    {
        /// <summary>
        /// Data result.
        /// </summary>
        /// <param name="data">Dữ liệu.</param>
        public DataResult(TData? data = default)
            : base(true, 200)
        {
            Data = data;
        }

        /// <summary>
        /// Dữ liệu.
        /// </summary>
        [JsonProperty(Order = -9)]
        [DisplayName("Dữ liệu")]
        public virtual TData? Data { get; set; }
    }
}
