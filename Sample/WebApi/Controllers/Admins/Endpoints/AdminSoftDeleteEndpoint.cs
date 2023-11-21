﻿namespace Sample.WebApi.Controllers.Admins
{
    public class AdminSoftDeleteEndpoint<TController, TEntity> : AdminController,
        IControllerEndpoint<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
    {
        public IStrongService<TEntity> Service { get; set; }

        [HttpDelete("{id}")]
        [SwaggerOperation("Khóa [controller]")]
        [Transactional]
        public async Task<SuccessResult> SoftDelete(RouteId route)
        {
            await Service.SoftDelete(route.Id);
            return SuccessResult();
        }

        [HttpPut("{id}/Restore")]
        [SwaggerOperation("Mở khóa [controller]")]
        [Transactional]
        public async Task<SuccessResult> Restore(RouteId route)
        {
            await Service.Restore(route.Id);
            return SuccessResult();
        }
    }

    public class AdminSoftDeleteEndpointAttribute<TController, TEntity> : BaseControllerEndpointAttribute<TController>
        where TController : BaseController
        where TEntity : class, IStrongEntity
    {
        public override TypeInfo ToEndpointTypeInfo()
        {
            return typeof(AdminSoftDeleteEndpoint<,>)
                .MakeGenericType(typeof(TController), typeof(TEntity))
                .GetTypeInfo();
        }
    }
}