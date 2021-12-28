using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace CpTech.Core.Dto
{
    public class BodyListId
    {
        [FromBody]
        [DisplayName("danh sách mã định danh")]
        public Guid[] ListId { get; set; }
    }
}