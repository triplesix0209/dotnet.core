using System;
using System.Linq;
using System.Threading.Tasks;
using Sample.Common.Dto;
using Sample.Data.Entities;
using Sample.Data.Repositories;
using Sample.Middle.Abstracts;

namespace Sample.Middle.Services
{
    public class SettingService : CommonService<SettingEntity, SettingAdminDto>,
        ISettingService
    {
        public SettingService(SettingRepository repo)
            : base(repo)
        {
        }

        public SettingRepository SettingRepo { get; set; }

        public async Task<string> GetValue(IIdentity identity, string code)
        {
            var setting = await SettingRepo.Query
                .WhereNotDeleted()
                .Where(x => x.Code == code)
                .FirstAsync<SettingEntity>(Mapper);

            return setting.Value;
        }

        public async Task<TValue> GetValue<TValue>(IIdentity identity, string code)
        {
            var value = await GetValue(identity, code);
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }
    }
}
