using Autofac;
using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminFilterDto<TEntity> : PagingInputDto, IQueryableDto<TEntity>
        where TEntity : class, IStrongEntity
    {
        public IQueryable<TEntity> ToQueryable(IQueryable<TEntity> query)
        {
            foreach (var property in GetType().GetProperties())
            {
                if (!property.PropertyType.IsAssignableTo<IFilterParameter>()) continue;
                var value = property.GetValue(this);
                if (value == null) continue;
                var method = property.PropertyType.GetMethod(nameof(IFilterParameter.ToQueryable))?
                    .MakeGenericMethod(typeof(TEntity));
                if (method == null) continue;

                var q = method.Invoke(value, new object[] { query });
            }

            return query;
        }
    }
}
