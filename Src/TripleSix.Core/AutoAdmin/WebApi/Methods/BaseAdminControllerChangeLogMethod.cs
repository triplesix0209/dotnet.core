using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Services;
using TripleSix.Core.WebApi.Results;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerChangeLogMethod<TEntity>
        : BaseAdminController
        where TEntity : class, IModelEntity
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpGet("{id}/ChangeLog")]
        [SwaggerApi("lấy lịch sử thay đổi của [controller]", typeof(PagingResult<ObjectLogDto>))]
        public virtual async Task<IActionResult> GetPageChangeLog(RouteId route, PagingFilterDto filter)
        {
            var identity = GenerateIdentity();
            var data = await Service.GetPageChangeLog(identity, route.Id, filter.Page, filter.Size);
            return PagingResult(data);
        }

        [HttpGet("ChangeLog/{objectLogId}")]
        [SwaggerApi("lấy chi tiết thay đổi của [controller]", typeof(DataResult<ObjectLogDto>))]
        public virtual async Task<IActionResult> GetDetailChangeLog([SwaggerParameter("mã định danh mục thay đổi")] Guid objectLogId)
        {
            var identity = GenerateIdentity();
            var data = await Service.GetChangeLog(identity, objectLogId);
            return DataResult(data);
        }
    }
}
