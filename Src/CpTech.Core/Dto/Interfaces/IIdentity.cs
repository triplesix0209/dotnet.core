using System;
using System.Security.Claims;

namespace CpTech.Core.Dto
{
    public interface IIdentity
    {
        ClaimsPrincipal User { get; }

        Guid? UserId { get; }
    }
}