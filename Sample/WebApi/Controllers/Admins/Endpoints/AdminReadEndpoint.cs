namespace Sample.WebApi.Controllers.Admins
{
    public class AdminReadEndpoint<TController, TEntity, TItem, TDetail, TFilter> : AdminController,
        IControllerEndpoint<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TItem : class, IDto
        where TDetail : class, IDto
        where TFilter : class, IQueryableDto<TEntity>, IPagingInput
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách [controller]")]
        public async Task<PagingResult<TItem>> GetPage(TFilter input)
        {
            var result = await Service.GetPageByQueryModel<TItem>(input, input.Page, input.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Lấy chi tiết [controller]")]
        public async Task<DataResult<TDetail>> GetDetail(RouteId route)
        {
            var result = await Service.GetFirstById<TDetail>(route.Id);
            return DataResult(result);
        }
    }

    public class AdminReadEndpointAttribute<TController, TEntity, TItem, TDetail, TFilter> : BaseControllerEndpointAttribute<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TItem : class, IDto
        where TDetail : class, IDto
        where TFilter : class, IQueryableDto<TEntity>, IPagingInput
    {
        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminReadEndpoint<,,,,>)
                .MakeGenericType(typeof(TController), typeof(TEntity), typeof(TItem), typeof(TDetail), typeof(TFilter))
                .GetTypeInfo();
        }
    }
}