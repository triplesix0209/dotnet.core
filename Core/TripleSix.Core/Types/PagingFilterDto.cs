using System.ComponentModel;

namespace TripleSix.Core.Types
{
    /// <summary>
    /// Id định danh.
    /// </summary>
    public class PagingFilterDto : IFilterDto
    {
        [DisplayName("số trang")]
        public virtual int Page { get; set; } = 1;

        [DisplayName("kích thước trang")]
        public virtual int Size { get; set; } = 10;
    }
}
