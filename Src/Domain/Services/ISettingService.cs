namespace Sample.Domain.Services
{
    public interface ISettingService : IStrongService<Setting>
    {
        Task<string?> GetValue(string code);

        Task<TValue?> GetValue<TValue>(string code);
    }
}
