namespace Sample.WebApi.Controllers.Commons
{
    public class AccountController : CommonController
    {
        public IAccountService? AccountService { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetPage(PagingFilterDto filter)
        {
            var result = await AccountService!.GetPage<AccountDto>(page: filter.Page, size: filter.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(RouteId route)
        {
            var result = await AccountService!.GetFirst(route.Id, false);
            return DataResult(result);
        }

        [HttpPost]
        [Transactional]
        public async Task<IActionResult> Create([FromBody] AccountDto input)
        {
            var result = await AccountService!.CreateWithMapper<AccountDto>(input);\
            return DataResult(result);
        }

        [HttpPut("{id}")]
        [Transactional]
        public async Task<IActionResult> Update(RouteId route, [FromBody] AccountDto input)
        {
            await AccountService!.UpdateWithMapper(route.Id, false, input);
            return SuccessResult();
        }
    }
}