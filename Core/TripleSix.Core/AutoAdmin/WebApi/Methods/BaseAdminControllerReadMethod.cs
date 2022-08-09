using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Persistences;
using TripleSix.Core.Services;
using TripleSix.Core.Types;
using TripleSix.Core.WebApi;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerReadMethod<TAdminDto, TEntity, TFilterDto, TItemDto, TDetailDto>
        : BaseAdminController
        where TAdminDto : IAdminModel
        where TEntity : IStrongEntity
        where TFilterDto : IDto
        where TItemDto : IDto
        where TDetailDto : IDto
    {
        //public IStrongService<TEntity, TDbDataContext> Service { get; set; }
        //public ICommonService<TEntity> Service { get; set; }

        //[HttpGet]
        //[SwaggerOperation("lấy danh sách [controller]")]
        //[AdminMethod(Type = AdminMethodTypes.List)]
        //public virtual async Task<PagingResult<TItemDto>> GetPage(TFilterDto input)
        //{
        //    //IPaging<TItemDto> result;
        //    //var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TItemDto));
        //    //if (Service.GetType().IsAssignableTo(readInterface))
        //    //    data = await Service.GetPageByFilterWithModel<TItemDto>(identity, input);
        //    //else
        //    //    data = await Service.GetPageByFilter<TItemDto>(identity, input);

        //    //return PagingResult(result);
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
