using System;
using System.ComponentModel;

namespace TripleSix.Core.Dto
{
    public class ModelFilterDto : PagingFilterDto,
        IModelFilterDto
    {
        [DisplayName("từ khóa tìm kiếm")]
        public virtual FilterParameterString Search { get; set; }

        [DisplayName("lọc theo mã định danh")]
        public virtual FilterParameter<Guid> Id { get; set; }

        [DisplayName("lọc theo mã số")]
        public virtual FilterParameterString Code { get; set; }

        [DisplayName("lọc theo trạng thái xóa")]
        public virtual FilterParameter<bool> IsDeleted { get; set; }

        [DisplayName("lọc theo ngày tạo")]
        public virtual FilterParameterDatetime CreateDatetime { get; set; }

        [DisplayName("lọc theo ngày sửa")]
        public virtual FilterParameterDatetime UpdateDatetime { get; set; }

        [DisplayName("lọc theo người tạo")]
        public virtual FilterParameter<Guid> CreatorId { get; set; }

        [DisplayName("lọc theo người sửa")]
        public virtual FilterParameter<Guid> UpdaterId { get; set; }
    }
}