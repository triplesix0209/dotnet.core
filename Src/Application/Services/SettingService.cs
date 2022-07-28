namespace Sample.Application.Services
{
    public class SettingService : StrongService<Setting>, ISettingService
    {
        public async Task<string?> GetValue(string code)
        {
            var setting = await Db.Setting
                .WhereNotDeleted()
                .Where(x => x.Code == code)
                .FirstOrDefaultAsync();
            if (setting == null)
                throw new EntityNotFoundException(typeof(Setting));

            return setting.Value;
        }

        public async Task<TValue?> GetValue<TValue>(string code)
        {
            var value = await GetValue(code);
            if (value == null) return default;

            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }
    }
}
