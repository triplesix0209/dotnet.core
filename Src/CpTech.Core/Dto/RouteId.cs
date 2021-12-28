using System;
using System.ComponentModel;

namespace CpTech.Core.Dto
{
    public class RouteId
    {
        [DisplayName("mã định danh")]
        public Guid Id { get; set; }
    }
}