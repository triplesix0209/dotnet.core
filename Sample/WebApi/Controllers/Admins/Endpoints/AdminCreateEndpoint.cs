namespace Sample.WebApi.Controllers.Admins
{
    public class AdminCreateEndpoint<TController, TEntity, TInput> : AdminController,
        IControllerEndpoint<TController, AdminCreateEndpointAttribute<TController, TEntity, TInput>>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpPost]
        [SwaggerOperation("Tạo [controller]")]
        [Transactional]
        public async Task<DataResult<Guid>> Create([FromBody] TInput input)
        {
            var result = await Service.CreateWithMapper(input);
            return DataResult(result.Id);
        }
    }

    public class AdminCreateEndpointAttribute<TController, TEntity, TInput>
        : BaseControllerEndpointAttribute
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public override Type EndpointType => typeof(AdminCreateEndpoint<TController, TEntity, TInput>);
    }
}