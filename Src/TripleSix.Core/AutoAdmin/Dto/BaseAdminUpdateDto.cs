using System.ComponentModel;
using TripleSix.Core.Dto;

namespace TripleSix.Core.AutoAdmin
{
    public class BaseAdminUpdateDto : DataDto,
        IAdminUpdateDto
    {
        [DisplayName("ghi chú chỉnh sửa")]
        public string ChangeLogNote { get; set; }
    }
}
