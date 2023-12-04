using System;
using System.ComponentModel;

namespace TripleSix.Core.Dto
{
    public class RouteId
    {
        [DisplayName("mã định danh")]
        public Guid Id { get; set; }
    }
}