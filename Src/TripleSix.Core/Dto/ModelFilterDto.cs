using System;
using System.ComponentModel;
using TripleSix.Core.Attributes;

namespace TripleSix.Core.Dto
{
    public class ModelFilterDto : PagingFilterDto,
        IModelFilterDto
    {
        [SwaggerHide]
        public virtual string AppendIds { get; set; }

        [DisplayName("lọc theo mã định danh")]
        public virtual Guid? Id { get; set; }

        [DisplayName("lọc theo mã định danh khác chỉ định")]
        public virtual Guid? NotId { get; set; }

        [DisplayName("lọc theo mã số")]
        public virtual string Code { get; set; }

        [DisplayName("lọc theo danh sách mã định danh")]
        public virtual string ListId { get; set; }

        [DisplayName("lọc theo danh sách mã định danh khác chỉ định")]
        public virtual string NotListId { get; set; }

        [DisplayName("lọc theo trạng thái xóa")]
        public virtual bool? IsDeleted { get; set; }

        [DisplayName("lọc theo thời gian tạo (bắt đầu)")]
        public virtual DateTime? StartCreateDatetime { get; set; }

        [DisplayName("lọc theo thời gian tạo (kết thúc)")]
        public virtual DateTime? EndCreateDatetime { get; set; }

        [DisplayName("lọc theo thời gian chỉnh sửa (bắt đầu)")]
        public virtual DateTime? StartUpdateDatetime { get; set; }

        [DisplayName("lọc theo thời gian chỉnh sửa (kết thúc)")]
        public virtual DateTime? EndUpdateDatetime { get; set; }

        [DisplayName("lọc theo người tạo")]
        public virtual Guid? CreatorId { get; set; }

        [DisplayName("lọc theo người sửa")]
        public virtual Guid? UpdaterId { get; set; }

        [DisplayName("sắp xếp theo cột")]
        public virtual string SortColumns { get; set; }

        [DisplayName("tìm kiếm")]
        public virtual string Search { get; set; }
    }
}