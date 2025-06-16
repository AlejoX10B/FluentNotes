using System.Threading.Tasks;

namespace FluentNotes.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task<T> GetConfigAsync<T>(string key, T defaultValue = default);
        Task SetConfigAsync<T>(string key, T value);
        Task<bool> IsFirstRunAsync();
        Task InitializeConfigsAsync();
    }
}
