namespace Sample.WebApi.Controllers.Commons
{
    public class AccountController : CommonController
    {
        public IAccountService? AccountService { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetPage(PagingFilterDto filter)
        {
            var client = new HttpClient();
            var data = await client.GetAsync("https://www.google.com/search?q=sample+api&oq=sample+api&aqs=chrome..69i57j0i512l2j0i22i30l3j0i10i22i30l4.3308j0j7&sourceid=chrome&ie=UTF-8");

            var result = await AccountService!.GetPage<AccountDto>(page: filter.Page, size: filter.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(RouteId route)
        {
            var result = await AccountService!.GetById<AccountDto>(route.Id, false);
            return DataResult(result);
        }

        [HttpPost]
        [Transactional]
        public async Task<IActionResult> Create([FromBody] AccountDto input)
        {
            var result = await AccountService!.CreateWithMapper<AccountDto>(input);
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