using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public class ChangeLogItemDto : BaseDto
    {
        [DisplayName("Id định danh")]
        public Guid Id { get; set; }
    }
}
