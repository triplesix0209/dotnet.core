using System.ComponentModel;
using Newtonsoft.Json;

namespace TripleSix.Core.WebApi
{
    /// <summary>
    /// Success result.
    /// </summary>
    public class SuccessResult : BaseResult<BaseMeta>
    {
        /// <summary>
        /// Success result.
        /// </summary>
        public SuccessResult()
            : base(true, 200)
        {
        }

        /// <summary>
        /// Success result.
        /// </summary>
        /// <param name="httpStatusCode">Mã trạng thái http.</param>
        public SuccessResult(int httpStatusCode)
            : base(true, httpStatusCode)
        {
        }

        /// <summary>
        /// Dữ liệu.
        /// </summary>
        [JsonProperty(Order = -9)]
        [DisplayName("Dữ liệu")]
        public virtual bool Data => true;
    }
}
