using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public class ChangeLogItemDto : BaseDto
    {
        [DisplayName("Id định danh")]
        public Guid Id { get; set; }

        [DisplayName("Ghi chú")]
        public string? Note { get; set; }

        [DisplayName("Thời gian thao tác")]
        public DateTime? CreateDateTime { get; set; }

        [DisplayName("Thông tin người thao tác")]
        public ActorDto? Actor { get; set; }
    }
}
