using System.ComponentModel;
using TripleSix.Core.Dto;

namespace TripleSix.AutoAdmin.Dto
{
    /// <summary>
    /// thông tin export.
    /// </summary>
    public class ExportInputDto : DataDto
    {
        [DisplayName("danh sách field")]
        public string[] ListField { get; set; }

        [DisplayName("số phút múi giờ so với UTC")]
        [DefaultValue(420)]
        public int? TimeOffset { get; set; } = 420;
    }
}
