using System;
using System.ComponentModel;
using TripleSix.CoreOld.Dto;

namespace TripleSix.CoreOld.AutoAdmin
{
    public class ActorDto : DataDto
    {
        [DisplayName("mã định danh người thao tác")]
        public Guid? Id { get; set; }

        [DisplayName("mã số người thao tác")]
        public string Code { get; set; }

        [DisplayName("tên người thao tác")]
        public string Name { get; set; }

        [DisplayName("avatar người thao tác")]
        public string AvatarLink { get; set; }
    }
}