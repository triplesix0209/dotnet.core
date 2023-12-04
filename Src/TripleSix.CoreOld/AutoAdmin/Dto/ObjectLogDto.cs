using System;
using System.ComponentModel;
using Newtonsoft.Json;
using TripleSix.CoreOld.Dto;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class ObjectLogDto : DataDto
    {
        [DisplayName("mã định danh")]
        public Guid Id { get; set; }

        [DisplayName("thời gian thao tác")]
        public DateTime Datetime { get; set; }

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

        [DisplayName("ghi chú")]
        public string Note { get; set; }

        [JsonIgnore]
        public Guid? ActorId { get; set; }
    }
}
