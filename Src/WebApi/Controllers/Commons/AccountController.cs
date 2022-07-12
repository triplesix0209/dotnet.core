namespace Sample.WebApi.Controllers.Commons
{
    public class AccountController : CommonController
    {
        public IAccountService? AccountService { get; set; }

        [HttpGet]
        public Task<IPaging<AccountDto>> GetPage(int page = 1, int size = 10)
        {
            return AccountService!.GetPage<AccountDto>(page: page, size: size);
        }

        [HttpPost]
        public Task<AccountDto> Create([FromBody] AccountDto input)
        {
            return AccountService!.CreateWithMapper<AccountDto>(input);
        }

        [HttpPut]
        public Task Update(Guid id, [FromBody] AccountDto input)
        {
            return AccountService!.UpdateWithMapper(id, false, input);
        }
    }
}