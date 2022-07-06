namespace Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Test()
        {
            return "Hello World";
        }
    }
}