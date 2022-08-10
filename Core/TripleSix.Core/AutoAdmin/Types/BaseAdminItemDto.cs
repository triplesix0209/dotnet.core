using Newtonsoft.Json;
using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminItemDto : BaseDto
    {
        [DisplayName("Id định danh")]
        [AdminProperty(Render = false)]
        public virtual Guid Id { get; set; }

        [DisplayName("Mục bị xóa?")]
        [AdminProperty(Render = false)]
        public virtual bool IsDeleted { get; set; }

        [DisplayName("Thời gian tạo")]
        public virtual DateTime? CreateDateTime { get; set; }

        [DisplayName("Thời gian sửa")]
        public virtual DateTime? UpdateDateTime { get; set; }
    }
}
