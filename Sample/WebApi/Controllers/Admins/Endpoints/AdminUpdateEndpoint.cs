namespace Sample.WebApi.Controllers.Admins
{
    public class AdminUpdateEndpoint<TController, TEntity, TInput> : AdminController,
        IControllerEndpoint<TController, AdminUpdateEndpointAttribute<TController, TEntity, TInput>>
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public IStrongServiceUpdate<TEntity, TInput> Service { get; set; }

        [HttpPut("{id}")]
        [SwaggerOperation("Sửa [controller]")]
        [Transactional]
        public async Task<SuccessResult> Update(RouteId route, [FromBody] TInput input)
        {
            var entity = await Service.GetFirstById(route.Id);
            await Service.Update(entity, input);
            return SuccessResult();
        }
    }

    public class AdminUpdateEndpointAttribute<TController, TEntity, TInput>
        : BaseControllerEndpointAttribute
        where TController : BaseController
        where TEntity : class, IStrongEntity
        where TInput : class, IDto
    {
        public override Type EndpointType => typeof(AdminUpdateEndpoint<TController, TEntity, TInput>);
    }
}