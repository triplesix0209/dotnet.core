using System;
using System.ComponentModel;

namespace TripleSix.Core.Dto
{
    public class RouteLinkId
    {
        [DisplayName("mã định danh đối tượng cần liên kết")]
        public Guid LinkId { get; set; }
    }
}