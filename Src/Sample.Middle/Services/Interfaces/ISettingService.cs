using System.Threading.Tasks;
using Sample.Data.Entities;
using TripleSix.CoreOld.AutoAdmin;
using TripleSix.CoreOld.Dto;

namespace Sample.Middle.Services
{
    public interface ISettingService : ICommonService<SettingEntity>
    {
        Task<string> GetValue(IIdentity identity, string code);

        Task<TValue> GetValue<TValue>(IIdentity identity, string code);
    }
}
