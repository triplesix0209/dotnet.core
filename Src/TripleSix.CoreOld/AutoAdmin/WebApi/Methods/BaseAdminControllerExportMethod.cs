using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.CoreOld.Attributes;
using TripleSix.CoreOld.Dto;
using TripleSix.CoreOld.Entities;
using TripleSix.CoreOld.Helpers;
using TripleSix.CoreOld.Services;
using TripleSix.CoreOld.WebApi.Filters;

namespace TripleSix.CoreOld.AutoAdmin
{
    public abstract class BaseAdminControllerExportMethod<TAdmin, TEntity, TFilterDto, TDetailDto>
        : BaseAdminController
        where TAdmin : class, IAdminDto
        where TEntity : class, IModelEntity
        where TFilterDto : IModelFilterDto
        where TDetailDto : class, IModelDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpGet("Export")]
        [SwaggerApi("xuất [controller]", typeof(File))]
        [AdminMethod(Type = AdminMethodTypes.Export)]
        [PermissionRequirement(AutoGroup = true, ListCode = new[] { "export", "read" })]
        public virtual async Task<IActionResult> Export(TFilterDto filter, ExportInputDto config)
        {
            var identity = GenerateIdentity();

            IModelDataDto[] data;
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TDetailDto));
            if (Service.GetType().IsAssignableTo(readInterface))
                data = await Service.GetListByFilterWithModel<TDetailDto>(identity, filter);
            else
                data = await Service.GetListByFilter<TDetailDto>(identity, filter);

            using (var workbook = await Service.Export(identity, data, config))
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var filename = typeof(TEntity).GetDisplayName() + "-" + DateTime.UtcNow.ToString("dd-MM-yyyy-HH-mm") + ".xlsx";
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
        }
    }
}
