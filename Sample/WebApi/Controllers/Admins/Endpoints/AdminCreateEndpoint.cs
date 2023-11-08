namespace Sample.WebApi.Controllers.Admins
{
    public class AdminCreateEndpoint<TBaseController, TEntity, TInput> : AdminController,
        IControllerEndpoint<TBaseController>
        where TBaseController : BaseController
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

    public class CreateEndpointAttribute : BaseControllerEndpointAttribute
    {
        public CreateEndpointAttribute(Type controllerType, Type entityType, Type inputType)
            : base(controllerType)
        {
            EntityType = entityType;
            InputType = inputType;
        }

        public Type EntityType { get; }

        public Type InputType { get; }

        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminCreateEndpoint<,,>)
                .MakeGenericType(ControllerType, EntityType, InputType)
                .GetTypeInfo();
        }
    }
}