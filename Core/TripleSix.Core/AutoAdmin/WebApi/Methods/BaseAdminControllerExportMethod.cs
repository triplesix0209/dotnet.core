using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerExportMethod<TEntity, TAdminModel, TFilterDto, TDetailDto>
        : BaseAdminController, IAdminMethod
        where TEntity : class, IStrongEntity
        where TAdminModel : AdminModel<TEntity>
        where TFilterDto : BaseAdminFilterDto<TEntity>
        where TDetailDto : BaseAdminItemDto
    {
        private static readonly string[] _excludeProperties = new[]
        {
            nameof(BaseAdminItemDto.IsDeleted),
            nameof(BaseAdminItemDto.UpdateDateTime),
        };

        public IStrongService<TEntity> Service { get; set; }

        [HttpGet("Export")]
        [SwaggerOperation("Xuất dữ liệu [controller]")]
        [AdminMethod(Type = AdminMethodTypes.Export)]
        public virtual async Task<FileContentResult> Export(TFilterDto filter, ExportConfigDto config)
        {
            var data = await Service.GetListByQueryModel<TDetailDto>(filter);

            var itemType = typeof(TDetailDto);
            var properties = itemType.GetProperties()
                .Where(x => !_excludeProperties.Contains(x.Name))
                .OrderBy(x => x.Name, new ExportPropertyComparer())
                .ToArray();

            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Data");
            for (var i = 0; i < properties.Length; i++)
                sheet.Cell(1, i + 1).Value = properties[i].GetDisplayName();

            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                for (var j = 0; j < properties.Length; j++)
                {
                    var property = properties[j];
                    var propertyType = property.PropertyType.GetUnderlyingType();
                    var value = itemType.GetProperty(property.Name)?.GetValue(item);

                    if (propertyType == typeof(bool))
                    {
                        var v = value as bool?;
                        if (!v.HasValue) continue;
                        if (v.Value) value = "có";
                        else value = "không";
                    }
                    else if (propertyType == typeof(DateTime))
                    {
                        var v = value as DateTime?;
                        if (!v.HasValue) continue;
                        value = v.Value.AddMinutes(config.TimezoneOffset);
                    }

                    sheet.Cell(i + 2, j + 1).Value = value;
                }
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var filename = typeof(TEntity).Name + "_" + DateTime.UtcNow.ToString("dd-MM-yyyy-HH-mm") + ".xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        private class ExportPropertyComparer : IComparer<string>
        {
            public int Compare(string? x, string? y)
            {
                if (x == nameof(BaseAdminItemDto.Id))
                    return -1;
                return 0;
            }
        }
    }
}
