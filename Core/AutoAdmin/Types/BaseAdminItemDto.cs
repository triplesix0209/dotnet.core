using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminItemDto : BaseDto
    {
        [DisplayName("Id định danh")]
        [AdminProperty(Render = false)]
        public virtual Guid Id { get; set; }

        [DisplayName("Thời gian xóa")]
        [AdminProperty(Render = false)]
        public virtual DateTime? DeleteAt { get; set; }

        [DisplayName("Thời gian tạo")]
        public virtual DateTime? CreateAt { get; set; }

        [DisplayName("Thời gian sửa")]
        public virtual DateTime? UpdateAt { get; set; }
    }
}
