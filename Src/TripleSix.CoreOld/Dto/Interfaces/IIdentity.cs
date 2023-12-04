using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TripleSix.CoreOld.Dto
{
    public interface IIdentity
    {
        HttpContext HttpContext { get; }

        ClaimsPrincipal User { get; }

        Guid? UserId { get; }

        string ClientId { get; set; }

        string IpAddress { get; set; }

        string RequestUrl { get; set; }

        string SubmitNote { get; set; }
    }
}
