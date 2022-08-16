using TripleSix.Core.Appsettings;

namespace Sample.WebApi.Controllers
{
    public class TestController : BaseController
    {
        public IdentityAppsetting IdentityAppsetting { get; set; }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return SuccessResult();
        }
    }
}