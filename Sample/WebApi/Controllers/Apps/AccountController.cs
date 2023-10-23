namespace Sample.WebApi.Controllers.Apps
{
    [SwaggerTag("tài khoản")]
    public class AccountController : AppController
    {
        public IAccountService AccountService { get; set; }
    }
}
