namespace Sample.WebApi.Controllers.Admins
{
    public class AdminReadEndpoint<TController, TEntity, TItem, TDetail, TFilter> : AdminController,
        IControllerEndpoint<TController, AdminReadEndpointAttribute<TController, TEntity, TItem, TDetail, TFilter>>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TItem : class, IDto
        where TDetail : class, IDto
        where TFilter : class, IEntityQueryableDto<TEntity>, IPagingInput
    {
        public IStrongServiceRead<TEntity, TFilter> Service { get; set; }

        [HttpPost("GetPage")]
        [SwaggerOperation("Lấy phân trang [controller]")]
        public async Task<PagingResult<TItem>> GetPage([FromBody] TFilter input)
        {
            var result = await Service.GetPage<TItem>(input, input.Page, input.Size);
            return PagingResult(result);
        }

        [HttpPost("GetAll")]
        [SwaggerOperation("Lấy phân trang [controller]")]
        public async Task<DataResult<List<TItem>>> GetAll([FromBody] TFilter input)
        {
            var result = await Service.GetList<TItem>(input);
            return DataResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Lấy chi tiết [controller]")]
        public async Task<DataResult<TDetail>> GetDetail(RouteId route)
        {
            var result = await Service.GetFirstById<TDetail>(route.Id);
            return DataResult(result);
        }
    }

    public class AdminReadEndpointAttribute<TController, TEntity, TItem, TDetail, TFilter>
        : BaseControllerEndpointAttribute
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TItem : class, IDto
        where TDetail : class, IDto
        where TFilter : class, IEntityQueryableDto<TEntity>, IPagingInput
    {
        public override Type EndpointType => typeof(AdminReadEndpoint<TController, TEntity, TItem, TDetail, TFilter>);
    }
}