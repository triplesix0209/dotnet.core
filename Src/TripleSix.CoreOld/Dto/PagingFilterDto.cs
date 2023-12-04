using System.ComponentModel;
using TripleSix.CoreOld.Attributes;

namespace TripleSix.CoreOld.Dto
{
    public class PagingFilterDto : FilterDto,
        IPagingFilterDto
    {
        [DisplayName("số trang")]
        [MinValidate(1)]
        public virtual int Page { get; set; } = 1;

        [DisplayName("kích thước trang")]
        [MinValidate(1)]
        public virtual int Size { get; set; } = 10;
    }
}