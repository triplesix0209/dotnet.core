#pragma warning disable SA1201 // Elements should appear in the correct order

using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
{
    public class SortColumn
    {
        [DisplayName("tên cột sẽ sắp xép")]
        [RequiredValidate]
        public string Name { get; set; }

        [DisplayName("hướng sắp xép")]
        [EnumValidate]
        public SortColumnOrder Order { get; set; } = SortColumnOrder.Asc;
    }

    public enum SortColumnOrder
    {
        Asc = 1,
        Desc = 2,
    }
}
