using System.ComponentModel;

namespace Sample.Common.Dto
{
    public class IdentityUpdateProfileDto : DataDto
    {
        [DisplayName("tên gọi")]
        [StringLengthValidate(100)]
        [RequiredValidate]
        public string Name { get; set; }

        [DisplayName("link ảnh đại điện")]
        public string AvatarLink { get; set; }
    }
}
