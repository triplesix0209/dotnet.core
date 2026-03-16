using System.ComponentModel;
using TripleSix.Core.Validation;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Form file upload.
    /// </summary>
    public class RoutePaging : IPagingInput
    {
        /// <inheritdoc/>
        [DisplayName("Vị trí trang")]
        [MinValue(1)]
        public int Page { get; set; } = 1;

        /// <inheritdoc/>
        [DisplayName("Kích thước trang")]
        [MinValue(1)]
        public int Size { get; set; } = 10;
    }
}
