namespace Sample.WebApi.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return SuccessResult();
        }
    }
}