namespace Sample.WebApi.Controllers.Commons
{
    public class AccountController : CommonController
    {
        public IAccountService? AccountService { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetPage(int page = 1, int size = 10)
        {
            var result = await AccountService!.GetPage<AccountDto>(page: page, size: size);
            return PagingResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountDto input)
        {
            var result = await AccountService!.CreateWithMapper<AccountDto>(input);
            return DataResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountDto input)
        {
            await AccountService!.UpdateWithMapper(id, false, input);
            return SuccessResult();
        }
    }
}