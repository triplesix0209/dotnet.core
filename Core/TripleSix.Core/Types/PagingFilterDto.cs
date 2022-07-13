using System.ComponentModel;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// DTO lọc.
    /// </summary>
    public class PagingFilterDto : BaseDto,
        IFilterDto
    {
        [DisplayName("số trang")]
        public virtual int Page { get; set; } = 1;

        [DisplayName("kích thước trang")]
        public virtual int Size { get; set; } = 10;
    }
}
