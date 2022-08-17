using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Thiết lập khi export.
    /// </summary>
    public class ExportConfigDto : BaseDto
    {
        [DisplayName("Số phút chênh lệch so với với múi giờ UTC")]
        public virtual int TimezoneOffset { get; set; } = 420;
    }
}
