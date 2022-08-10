using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TripleSix.Core.Types
{
    public enum SortColumnOrder
    {
        [Description("tăng dần")]
        Asc = 1,

        [Description("giảm dần")]
        Desc = 2,
    }

    public class SortColumn : BaseDto
    {
        [DisplayName("tên cột sẽ sắp xép")]
        [Required]
        public string? Name { get; set; }

        [DisplayName("hướng sắp xép")]
        public SortColumnOrder Order { get; set; } = SortColumnOrder.Asc;
    }
}
