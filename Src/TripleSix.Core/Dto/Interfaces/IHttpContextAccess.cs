using Microsoft.AspNetCore.Http;

namespace TripleSix.Core.Dto
{
    public interface IHttpContextAccess
    {
        void SetHttpContext(HttpContext httpContext);

        HttpContext GetHttpContext();
    }
}
