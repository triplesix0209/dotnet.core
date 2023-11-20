namespace Sample.WebApi.Controllers.Admins
{
    public class AdminUpdateEndpoint<TController, TEntity, TInput> : AdminController,
        IControllerEndpoint<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa [controller]")]
        [Transactional]
        public async Task<SuccessResult> Update(RouteId route, [FromBody] TInput input)
        {
            await Service.UpdateWithMapper(route.Id, input);
            return SuccessResult();
        }
    }

    public class AdminUpdateEndpointAttribute<TController, TEntity, TInput> : BaseControllerEndpointAttribute<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminUpdateEndpoint<,,>)
                .MakeGenericType(typeof(TController), typeof(TEntity), typeof(TInput))
                .GetTypeInfo();
        }
    }
}