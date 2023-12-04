﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Common;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.WebApi.Controllers;

namespace Sample.WebApi.Abstracts
{
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "common")]
    [Authorize(AuthenticationSchemes = "account-token")]
    [AllowAnonymous]
    public abstract class CommonController : BaseController
    {
        protected override IIdentity GenerateIdentity()
        {
            return new Identity(HttpContext);
        }
    }
}
