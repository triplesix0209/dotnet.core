using System.ComponentModel;
using Autofac;
using TripleSix.Core.Entities;
using TripleSix.Core.Types;

namespace TripleSix.Core.AutoAdmin
{
    public abstract class BaseAdminFilterDto<TEntity> : PagingInputDto, IQueryableDto<TEntity>
        where TEntity : class, IStrongEntity
    {
        [DisplayName("Lọc theo Id")]
        public FilterParameter<Guid> Id { get; set; }

        [DisplayName("Lọc theo trạng thái đã xóa")]
        public FilterParameter<bool> IsDeleted { get; set; }

        [DisplayName("Lọc theo thời gian tạo")]
        public FilterParameter<DateTime> CreateDateTime { get; set; }

        [DisplayName("Lọc theo thời gian sửa")]
        public FilterParameter<DateTime> UpdateDateTime { get; set; }

        [DisplayName("Lọc theo người tạo")]
        public FilterParameter<Guid> CreatorId { get; set; }

        [DisplayName("Lọc theo người sửa")]
        public FilterParameter<Guid> UpdatorId { get; set; }

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

                if (method.Invoke(value, new object[] { query, property.Name }) is not IQueryable<TEntity> filteredQuery)
                    continue;
                query = filteredQuery;
            }

            return query;
        }
    }
}
