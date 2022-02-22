﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripleSix.Core.Attributes;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminControllerExportMethod<TEntity, TFilterDto, TDetailDto>
        : BaseAdminController
        where TEntity : class, IModelEntity
        where TFilterDto : IModelFilterDto
        where TDetailDto : class, IModelDataDto
    {
        public ICommonService<TEntity> Service { get; set; }

        [HttpGet("Export")]
        [SwaggerApi("xuất [controller]", typeof(File))]
        public virtual async Task<IActionResult> Export(TFilterDto filter, ExportInputDto config)
        {
            var identity = GenerateIdentity();

            IModelDataDto[] data;
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), typeof(TDetailDto));
            if (readInterface.IsAssignableFrom(Service.GetType()))
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