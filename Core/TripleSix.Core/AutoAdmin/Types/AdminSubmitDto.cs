using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TripleSix.Core.Types;
using TripleSix.Core.Validation;

namespace TripleSix.Core.AutoAdmin
{
    public class AdminSubmitDto<T> : BaseDto
        where T : IDto
    {
        [DisplayName("Dữ liệu")]
        [Required]
        public T Data { get; set; }

        [DisplayName("Ghi chú")]
        [NotEmpty]
        public string? Note { get; set; }
    }
}
