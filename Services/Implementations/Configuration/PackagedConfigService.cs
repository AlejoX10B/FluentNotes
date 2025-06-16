using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class PackagedConfigService : IConfigurationService
    {
        private readonly ApplicationDataContainer _localSettings;

        public PackagedConfigService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public async Task<T> GetConfigAsync<T>(string key, T defaultValue = default)
        {
            try
            {
                if (_localSettings.Values.TryGetValue(key, out var value))
                    return (T)value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo la configuración para la clave '{key}': {ex.Message}");
            }

            return defaultValue;
        }

        public async Task SetConfigAsync<T>(string key, T value)
        {
            try
            {
                _localSettings.Values[key] = value;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo la versión de la app: {key}': {ex.Message}");
            }
        }

        public async Task<bool> IsFirstRunAsync() =>
            await GetConfigAsync(ConfigKeys.IsFirstRun, true);

        public async Task InitializeConfigsAsync()
        {
            if (!await IsFirstRunAsync())
                return;

            await SetConfigAsync(ConfigKeys.IsFirstRun, true);
            await SetConfigAsync(ConfigKeys.IsOnboardingCompleted, false);
            await SetConfigAsync(ConfigKeys.AppVersion, GetAppVersion());
        }

        private string GetAppVersion()
        {
            try
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo la versión de la app: {ex.Message}");
                return "1.0.0";
            }
        }

    }
}
