using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerReadMethod<TAdminDto, TEntity, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminController
        where TAdminDto : IAdminDto
        where TEntity : IStrongEntity
        where TFilterDto : IDto
        where TItemDto : IDto
        where TDetailDto : IDto
    {
        //public ICommonService<TEntity> Service { get; set; }

        //[HttpGet]
        //[SwaggerApi("lấy danh sách [controller]")]
        //[AdminMethod(Type = AdminMethodTypes.List)]
        //[PermissionRequirement(AutoGroup = true, ListCode = new[] { "read" })]
        //public virtual async Task<IActionResult> GetPage(TFilterDto input)
        //{
        //    var identity = GenerateIdentity();

        //    IPaging<TItemDto> data;
        //    var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TItemDto));
        //    if (Service.GetType().IsAssignableTo(readInterface))
        //        data = await Service.GetPageByFilterWithModel<TItemDto>(identity, input);
        //    else
        //        data = await Service.GetPageByFilter<TItemDto>(identity, input);

        //    return PagingResult(data);
        //}

        //[HttpGet("{id}")]
        //[SwaggerApi("lấy chi tiết [controller]")]
        //[AdminMethod(Type = AdminMethodTypes.Detail)]
        //[PermissionRequirement(AutoGroup = true, ListCode = new[] { "read" })]
        //public virtual async Task<IActionResult> GetDetail(RouteId route)
        //{
        //    var identity = GenerateIdentity();

        //    TDetailDto data;
        //    var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TDetailDto));
        //    if (Service.GetType().IsAssignableTo(readInterface))
        //        data = await Service.GetFirstByIdWithModel<TDetailDto>(identity, route.Id, true);
        //    else
        //        data = await Service.GetFirstById<TDetailDto>(identity, route.Id, true);

        //    return DataResult(data);
        //}
    }
}
