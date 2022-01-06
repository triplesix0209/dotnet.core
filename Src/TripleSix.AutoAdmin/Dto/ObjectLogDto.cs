using System;
using System.ComponentModel;
using TripleSix.Core.Dto;

namespace TripleSix.AutoAdmin.Dto
{
    /// <summary>
    /// thông tin change log của đối tượng.
    /// </summary>
    public class ObjectLogDto : ModelDataDto
    {
        [DisplayName("loại đối tượng")]
        public string ObjectType { get; set; }

        [DisplayName("mã đối tượng")]
        public Guid ObjectId { get; set; }

        [DisplayName("mã định danh người thao tác")]
        public Guid? ActorId { get; set; }

        [DisplayName("mã số người thao tác")]
        public string ActorCode { get; set; }

        [DisplayName("tên người thao tác")]
        public string ActorName { get; set; }

        [DisplayName("avatar người thao tác")]
        public string ActorAvatarLink { get; set; }

        [DisplayName("dữ liệu đối tượng trước khi thay đổi")]
        public string BeforeData { get; set; }

        [DisplayName("dữ liệu đối tượng sau khi thay đổi")]
        public string AfterData { get; set; }
    }
}