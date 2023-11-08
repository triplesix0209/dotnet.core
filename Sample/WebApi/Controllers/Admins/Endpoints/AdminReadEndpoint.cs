namespace Sample.WebApi.Controllers.Admins
{
    public class AdminReadEndpoint<TController, TEntity, TData, TFilter> : AdminController,
        IControllerEndpoint<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TData : class, IDto
        where TFilter : class, IQueryableDto<TEntity>, IPagingInput
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách [controller]")]
        public async Task<PagingResult<TData>> GetPage(TFilter input)
        {
            var result = await Service.GetPageByQueryModel<TData>(input, input.Page, input.Size);
            return PagingResult(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Lấy chi tiết [controller]")]
        public async Task<DataResult<AccountDataAdminDto>> GetDetail(RouteId route)
        {
            var result = await Service.GetFirstById<AccountDataAdminDto>(route.Id);
            return DataResult(result);
        }
    }

    public class ReadEndpointAttribute<TController, TEntity, TData, TFilter> : BaseControllerEndpointAttribute<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TData : class, IDto
        where TFilter : class, IQueryableDto<TEntity>, IPagingInput
    {
        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminReadEndpoint<,,,>)
                .MakeGenericType(typeof(TController), typeof(TEntity), typeof(TData), typeof(TFilter))
                .GetTypeInfo();
        }
    }
}