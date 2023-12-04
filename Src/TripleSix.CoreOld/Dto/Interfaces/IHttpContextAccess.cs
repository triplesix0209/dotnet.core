using Microsoft.AspNetCore.Http;

namespace TripleSix.CoreOld.Dto
{
    public interface IHttpContextAccess
    {
        void SetHttpContext(HttpContext httpContext);

        HttpContext GetHttpContext();
    }
}
