using TripleSix.Core.Types;

namespace Sample.WebApi.Controllers.Commons
{
    public class AccountController : CommonController
    {
        public IAccountService? AccountService { get; set; }

        [HttpGet]
        public async Task<IPaging<AccountDto>> GetPage(int page = 1, int size = 10)
        {
            var result = await AccountService!.GetPage<AccountDto>(page: page, size: size);
            return result;
        }

        [HttpPost]
        public async Task<AccountDto> Create([FromBody] AccountDto input)
        {
            var result = await AccountService!.CreateWithMapper<AccountDto>(input);
            return result;
        }

        [HttpPut]
        public async Task Update(Guid id, [FromBody] AccountDto input)
        {
            await AccountService!.UpdateWithMapper(id, false, input);
        }
    }
}