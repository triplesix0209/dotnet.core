namespace Sample.WebApi.Controllers.Admins
{
    public class AdminReadEndpoint<TBaseController, TEntity, TData, TFilter> : AdminController,
        IControllerEndpoint<TBaseController>
        where TBaseController : BaseController
        where TEntity : class, IStrongEntity
        where TData : class, IDto
        where TFilter : class, IQueryableDto<TEntity>
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpGet]
        [SwaggerOperation("Lấy danh sách [controller]")]
        public async Task<PagingResult<TData>> GetPage(TFilter input)
        {
            var result = await Service.GetPageByQueryModel<TData>(input, 1, 10);
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

    public class ReadEndpointAttribute : BaseControllerEndpointAttribute
    {
        public ReadEndpointAttribute(Type controllerType, Type entityType, Type dataType, Type filterType)
            : base(controllerType)
        {
            EntityType = entityType;
            DataType = dataType;
            FilterType = filterType;
        }

        public Type EntityType { get; }

        public Type DataType { get; }

        public Type FilterType { get; }

        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminReadEndpoint<,,,>)
                .MakeGenericType(ControllerType, EntityType, DataType, FilterType)
                .GetTypeInfo();
        }
    }
}