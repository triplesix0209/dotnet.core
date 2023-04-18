using System.Linq.Expressions;

namespace Sample.Application.Services
{
    public interface ISettingService : IStrongService<Setting>
    {
        Task<string?> GetValue(Expression<Func<DbSettings, DbSettingItem>> selector);

        Task<TValue?> GetValue<TValue>(Expression<Func<DbSettings, DbSettingItem>> selector);
    }

    public class SettingService : StrongService<Setting>, ISettingService
    {
        public SettingService(IDbDataContext db)
            : base(db)
        {
        }

        public async Task<string?> GetValue(Expression<Func<DbSettings, DbSettingItem>> selector)
        {
            var code = (selector?.Body as MemberExpression)?.Member.Name;
            if (code.IsNullOrEmpty()) throw new EntityNotFoundException(typeof(Setting));

            var query = Query
                .Where(x => x.Code == code);
            var setting = await GetFirst(query);

            return setting.Value;
        }

        public async Task<TValue?> GetValue<TValue>(Expression<Func<DbSettings, DbSettingItem>> selector)
        {
            var value = await GetValue(selector);
            if (value == null) return default;

            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }
    }
}
