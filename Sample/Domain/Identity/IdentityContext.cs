﻿using Microsoft.AspNetCore.Http;
using TripleSix.Core.Identity;

namespace Sample.Domain.Identity
{
    public class IdentityContext : BaseIdentityContext
    {
        public IdentityContext(HttpContext? httpContext)
            : base(httpContext)
        {
        }
    }
}
