using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Dto
{
    public interface IIdentity
    {
        HttpContext HttpContext { get; }

        ClaimsPrincipal User { get; }

        Guid? UserId { get; }
    }
}