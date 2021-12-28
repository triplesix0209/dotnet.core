using System.ComponentModel;
using CpTech.Core.Attributes;

namespace CpTech.Core.Dto
{
    public class PagingFilterDto : FilterDto,
        IPagingFilterDto
    {
        [DisplayName("số trang")]
        [DefaultValue(1)]
        [MinValidate(1)]
        public virtual int Page { get; set; } = 1;

        [DisplayName("kích thước trang")]
        [DefaultValue(10)]
        [RangeValidate(1, 10000)]
        public virtual int Size { get; set; } = 10;
    }
}