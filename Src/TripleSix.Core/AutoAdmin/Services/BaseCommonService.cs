using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Helpers;
using TripleSix.Core.JsonSerializers.ContractResolvers;
using TripleSix.Core.JsonSerializers.Converters;
using TripleSix.Core.Repositories;
using TripleSix.Core.Services;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseCommonService<TEntity> : ModelService<TEntity>,
        ICommonService<TEntity>
        where TEntity : class, IModelEntity
    {
        protected BaseCommonService(IModelRepository<TEntity> repo)
            : base(repo)
        {
        }

        public ObjectLogRepository ObjectLogRepo { get; set; }

        public virtual async Task<ObjectLogDto> GetChangeLog(IIdentity identity, Guid objectLogId)
        {
            var objectLog = await ObjectLogRepo.Query
                .Where(x => x.ObjectType == typeof(TEntity).Name)
                .Where(x => x.Id == objectLogId)
                .FirstAsync<ObjectLogDto>(Mapper);

            if (objectLog.CreatorId.HasValue)
                objectLog.Actor = (await GetActor(identity, objectLog.CreatorId.Value))?.First();

            return objectLog;
        }

        public virtual async Task<IPaging<ObjectLogDto>> GetPageChangeLog(IIdentity identity, Guid id, int page, int size = 10)
        {
            var objectLogs = await ObjectLogRepo.Query
                .Where(x => x.ObjectType == typeof(TEntity).Name)
                .Where(x => x.ObjectId == id)
                .OrderByDescending(x => x.Datetime)
                .ToPagingAsync<ObjectLogDto>(Mapper, page, size);

            var actorIds = objectLogs.Items
                .Where(x => x.CreatorId.HasValue)
                .Select(x => x.CreatorId.Value)
                .Distinct()
                .ToArray();

            if (actorIds.IsNotNullOrEmpty())
            {
                var actors = await GetActor(identity, actorIds);
                if (actors.IsNotNullOrEmpty())
                {
                    foreach (var objectLog in objectLogs.Items)
                    {
                        if (objectLog.CreatorId.HasValue)
                            objectLog.Actor = actors.FirstOrDefault(x => x.Id == objectLog.CreatorId.Value);
                    }
                }
            }

            return objectLogs;
        }

        public virtual async Task WriteChangeLog(IIdentity identity, Guid id, string beforeData = null)
        {
            var entity = await GetFirstById(identity, id);
            var afterData = await SerializeEntity(identity, entity);
            if (beforeData is not null && beforeData == afterData) return;

            ObjectLogRepo.Create(new ObjectLogEntity
            {
                Datetime = DateTime.UtcNow,
                ActorId = identity.UserId,
                ObjectType = typeof(TEntity).GetDisplayName(),
                ObjectId = id,
                BeforeData = beforeData,
                AfterData = afterData,
            });
            await ObjectLogRepo.SaveChanges();
        }

        public virtual Task<XLWorkbook> Export(IIdentity identity, IModelDataDto[] data, ExportInputDto config)
        {
            #region [chuẩn bị danh sách property]

            var itemType = data.GetType().GetElementType();
            var properties = itemType.GetProperties()
                .Where(x => new[]
                {
                    nameof(IModelDataDto.UpdateDatetime),
                    nameof(IModelDataDto.CreatorId),
                    nameof(IModelDataDto.UpdaterId),
                }.Contains(x.Name) == false)
                .ToArray();

            if (config.ListField.IsNotNullOrEmpty())
            {
                properties = properties
                    .Where(x => config.ListField.Any(y => y.Trim().ToLower() == x.Name.ToLower()))
                    .ToArray();
            }

            properties = properties
                .OrderBy(x => x.Name, new ExportPropertyComparer())
                .ToArray();

            #endregion

            var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add("Data");
            for (var i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var displayName = property.GetCustomAttribute<DisplayNameAttribute>();
                sheet.Cell(1, i + 1).Value = displayName is null ? property.Name : displayName.DisplayName;
            }

            for (var i = 0; i < data.Length; i++)
            {
                var item = data[i];
                for (var j = 0; j < properties.Length; j++)
                {
                    var property = properties[j];
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    var value = itemType.GetProperty(property.Name).GetValue(item);

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
                        value = v.Value.AddMinutes(config.TimeOffset ?? 420);
                    }

                    sheet.Cell(i + 2, j + 1).Value = value;
                }
            }

            sheet.Columns().AdjustToContents();
            return Task.FromResult(workbook);
        }

        public override Task<string> GenerateCode(IIdentity identity, TEntity entity)
        {
            return Task.FromResult(RandomHelper.RandomString(10));
        }

        protected virtual Task<string> SerializeEntity(IIdentity identity, TEntity entity)
        {
            return SerializeData(entity);
        }

        protected virtual Task<string> SerializeData(object data, params string[] excludeProperties)
        {
            var jsonText = JsonConvert.SerializeObject(data, EntityContractResolver.SerializerSettings);
            if (excludeProperties.IsNullOrEmpty())
                return Task.FromResult(jsonText);

            var jsonObject = JObject.Parse(jsonText);
            foreach (var key in excludeProperties)
                jsonObject.Remove(key.ToCamelCase());

            jsonText = jsonObject.ToString(Formatting.None, EntityContractResolver.SerializerSettings.Converters.ToArray());
            return Task.FromResult(jsonText);
        }

        protected abstract Task<IEnumerable<ActorDto>> GetActor(IIdentity identity, params Guid[] actorIds);

        private class EntityContractResolver : BaseContractResolver
        {
            public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new EntityContractResolver(),
                Converters = new JsonConverter[] { new TimestampConverter() },
            };

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                if (new[]
                {
                    nameof(IModelEntity.Id),
                    nameof(IModelEntity.CreateDatetime),
                    nameof(IModelEntity.UpdateDatetime),
                    nameof(IModelEntity.CreatorId),
                    nameof(IModelEntity.UpdaterId),
                }.Contains(member.Name))
                {
                    property.ShouldSerialize = _ => false;
                    return property;
                }

                var type = (member as PropertyInfo)?.PropertyType;
                if (type != null && (typeof(IEntity).IsAssignableFrom(type) || type.IsSubclassOfRawGeneric(typeof(IList<>))))
                {
                    property.ShouldSerialize = _ => false;
                    return property;
                }

                return property;
            }
        }

        private class ExportPropertyComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == nameof(IModelEntity.Code) && y == nameof(IModelEntity.Id))
                    return 1;
                if (new[] { nameof(IModelEntity.Id), nameof(IModelEntity.Code) }.Contains(x))
                    return -1;
                return 0;
            }
        }
    }
}
