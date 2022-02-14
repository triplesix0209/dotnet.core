using System;
using System.ComponentModel;
using TripleSix.Core.Dto;

namespace TripleSix.Core.AutoAdmin
{
    public class ObjectLogDto : ModelDataDto
    {
        [DisplayName("loại đối tượng")]
        public string ObjectType { get; set; }

        [DisplayName("mã đối tượng")]
        public Guid ObjectId { get; set; }

        [DisplayName("dữ liệu đối tượng trước khi thay đổi")]
        public string BeforeData { get; set; }

        [DisplayName("dữ liệu đối tượng sau khi thay đổi")]
        public string AfterData { get; set; }

        [DisplayName("thông tin người thao tác")]
        public ActorDto Actor { get; set; }
    }
}