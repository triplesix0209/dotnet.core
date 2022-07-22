using TripleSix.Core.Helpers;

namespace Sample.WebApi.Controllers.Commons
{
    public class TestController : BaseController
    {
        [HttpGet("Curl")]
        public async Task<IActionResult> Curl()
        {
            var result = await HttpContext.Request.ToCurl();
            return DataResult(result);
        }
    }
}