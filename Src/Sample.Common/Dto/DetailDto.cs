using System.Collections.Generic;
using System.ComponentModel;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;

namespace Sample.Common.Dto
{
    public class DetailDto : DataDto
    {
        [DisplayName("tên gọi")]
        [StringLengthValidate(100)]
        public string Name { get; set; }

        [DisplayName("danh sách mã số")]
        public List<int> Codes { get; set; } = new List<int>() { 1, 2, 3 };
    }
}
