using System.ComponentModel;

namespace TripleSix.Core.AutoAdmin
{
    public class ChangeLogDetailDto : ChangeLogItemDto
    {
        [DisplayName("Danh sách field thay đổi")]
        public ChangeLogField[] ListField { get; set; }
    }

    public class ChangeLogField
    {
        [DisplayName("Tên field")]
        public string FieldName { get; set; }

        [DisplayName("Giá trị cũ")]
        public string? OldValue { get; set; }

        [DisplayName("Giá trị mới")]
        public string? NewValue { get; set; }
    }
}
