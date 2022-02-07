using Microsoft.AspNetCore.Http;
using TripleSix.Core.Dto;

namespace Sample.Common
{
    public class Identity : BaseIdentity
    {
        public Identity()
            : base()
        {
        }

        public Identity(HttpContext httpContext)
            : base(httpContext)
        {
        }
    }
}
