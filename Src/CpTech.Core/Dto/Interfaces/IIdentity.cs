using System;
using System.Security.Claims;
using CpTech.Core.Enums;

namespace CpTech.Core.Dto
{
    public interface IIdentity
    {
        ClaimsPrincipal User { get; }

        Guid? UserId { get; }

        ClientDeviceType ClientDeviceType { get; }
    }
}