using System;
using System.Linq;
using System.Threading.Tasks;
using TripleSix.AutoAdmin.Dto;
using TripleSix.Core.Dto;
using TripleSix.Core.Entities;
using TripleSix.Core.Mappers;
using TripleSix.Core.Repositories;
using TripleSix.Core.Services;

namespace TripleSix.AutoAdmin.Services
{
    public abstract class BaseCommonService<TEntity, TAdminDto> : BaseCommonService<TEntity>
        where TEntity : class, IModelEntity
        where TAdminDto : BaseAdminDto
    {
        protected BaseCommonService(IModelRepository<TEntity> repo)
            : base(repo)
        {
        }

        protected override async Task<string> SerializeEntity(IIdentity identity, TEntity entity)
        {
            var detailType = typeof(TAdminDto).GetNestedTypes().FirstOrDefault(x => x.Name == "Detail");
            if (detailType is null) return await base.SerializeEntity(identity, entity);

            var serviceType = GetType();
            var readInterface = typeof(IReadableWithModel<,>).MakeGenericType(typeof(TEntity), detailType);
            if (readInterface.IsAssignableFrom(serviceType))
            {
                var method = readInterface.GetMethod(nameof(IReadableWithModel<IModelEntity, IModelDataDto>.ConvertEntityToModel));
                var task = method.Invoke(this, new object[] { identity, entity, null });

                var taskType = typeof(Task<>).MakeGenericType(detailType);
                taskType.GetMethod(nameof(Task<object>.Wait), new Type[0]).Invoke(task, new object[0]);
                var result = taskType.GetProperty(nameof(Task<object>.Result)).GetValue(task);
                return await SerializeData(result);
            }

            return await SerializeData(Mapper.MapData(entity, typeof(TEntity), detailType));
        }
    }
}
