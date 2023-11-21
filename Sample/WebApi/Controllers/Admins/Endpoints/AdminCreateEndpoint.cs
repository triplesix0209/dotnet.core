namespace Sample.WebApi.Controllers.Admins
{
    public class AdminCreateEndpoint<TController, TEntity, TInput> : AdminController,
        IControllerEndpoint<TController>
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

    public class AdminCreateEndpointAttribute<TController, TEntity, TInput> : BaseControllerEndpointAttribute<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        /// <inheritdoc/>
        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminCreateEndpoint<,,>)
                .MakeGenericType(typeof(TController), typeof(TEntity), typeof(TInput))
                .GetTypeInfo();
        }
    }
}