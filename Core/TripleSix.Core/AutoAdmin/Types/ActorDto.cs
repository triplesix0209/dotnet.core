using System.ComponentModel;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    /// <summary>
    /// Thông tin người thao tác.
    /// </summary>
    public class ActorDto : BaseDto
    {
        [DisplayName("Id người thao tác")]
        public Guid Id { get; set; }

        [DisplayName("Tên người thao tác")]
        public string? Name { get; set; }
    }
}
