using System;
using System.ComponentModel;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class SampleDto : DataDto
    {
        [DisplayName("mã định danh")]
        public Guid Id { get; set; }

        [DisplayName("phân loại")]
        public Types Type { get; set; }

        [DisplayName("chi tiết")]
        public DetailDto Detail { get; set; }
    }
}
