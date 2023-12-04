using System;
using System.ComponentModel;

namespace TripleSix.CoreOld.Dto
{
    public class RouteId
    {
        [DisplayName("mã định danh")]
        public Guid Id { get; set; }
    }
}