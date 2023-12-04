using System;
using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.AutoAdmin;

namespace TripleSix.CoreOld.Dto
{
    public class ModelDataDto : DataDto,
        IModelDataDto
    {
        [DisplayName("mã định danh")]
        [JsonProperty(Order = -10)]
        [AdminField(Render = false)]
        public virtual Guid? Id { get; set; }

        [DisplayName("đã bị xóa?")]
        [JsonProperty(Order = -10)]
        public virtual bool? IsDeleted { get; set; }

        [DisplayName("thời gian tạo")]
        [JsonProperty(Order = -10)]
        public virtual DateTime? CreateDatetime { get; set; }

        [DisplayName("thời gian sửa")]
        [JsonProperty(Order = -10)]
        public virtual DateTime? UpdateDatetime { get; set; }

        [DisplayName("mã định danh người tạo")]
        [JsonProperty(Order = -10)]
        public virtual Guid? CreatorId { get; set; }

        [DisplayName("mã định danh người sửa")]
        [JsonProperty(Order = -10)]
        public virtual Guid? UpdaterId { get; set; }

        [DisplayName("mã số")]
        [JsonProperty(Order = -10)]
        [StringLengthValidate(100)]
        public virtual string Code { get; set; }
    }
}
