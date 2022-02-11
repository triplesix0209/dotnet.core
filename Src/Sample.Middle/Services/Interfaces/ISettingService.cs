using System.Threading.Tasks;
using Sample.Data.Entities;
using TripleSix.Core.Dto;
using TripleSix.Core.Services;

namespace Sample.Middle.Services
{
    public interface ISettingService : IModelService<SettingEntity>
    {
        Task<string> GetValue(IIdentity identity, string code);

        Task<TValue> GetValue<TValue>(IIdentity identity, string code);
    }
}
