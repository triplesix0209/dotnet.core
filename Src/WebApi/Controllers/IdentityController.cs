namespace Sample.WebApi.Controllers.Commons
{
    public class IdentityController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] IdentityLoginDto input)
        {
            //var result = await AccountService!.CreateWithMapper<AccountDto>(input);
            //return DataResult(result);

            return SuccessResult();
        }
    }
}