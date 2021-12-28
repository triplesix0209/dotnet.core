using System;
using System.Security.Claims;

namespace TripleSix.Core.Dto
{
    public interface IIdentity
    {
        ClaimsPrincipal User { get; }

        Guid? UserId { get; }
    }
}