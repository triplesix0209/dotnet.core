using TripleSix.Core.Services.Interfaces;

namespace Sample.Domain.Services
{
    public interface IAccountService : IService
    {
        Task<string> Test();
    }
}
