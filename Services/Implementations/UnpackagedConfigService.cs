using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluentNotes.Services.Implementations
{
    internal class UnpackagedConfigService : IConfigurationService
    {
        private readonly string _configFilePath;
        private Dictionary<string, object> _configStore;

        public UnpackagedConfigService(string appName = "FluentNotes")
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDirectory = Path.Combine(appDataPath, appName);
            _configFilePath = Path.Combine(appDirectory, "configs.json");

            Directory.CreateDirectory(appDirectory);
            LoadConfigsFromFile();
        }


        public async Task<T> GetConfigAsync<T>(string key, T defaultValue = default)
        {
            try
            {
                if (_configStore.TryGetValue(key, out var value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo la configuración para la clave '{key}': {ex.Message}");
            }

            return defaultValue;
        }

        public async Task SetConfigAsync<T>(string key, T value)
        {
            try
            {
                _configStore[key] = value;
                await SaveConfigsToFileAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando la configuración para la clave '{key}': {ex.Message}");
            }
        }

        public async Task InitializeConfigsAsync()
        {
            if (!await IsFirstRunAsync())
                return;

            await SetConfigAsync(ConfigKeys.IsFirstRun, true);
            await SetConfigAsync(ConfigKeys.AppVersion, GetAppVersion());

            await SaveConfigsToFileAsync();
        }

        public async Task<bool> IsFirstRunAsync() =>
            await GetConfigAsync<bool>(ConfigKeys.IsFirstRun, true);

        private string GetAppVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                return assembly.GetName().Version?.ToString() ?? "1.0.0";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo la versión de la app: {ex.Message}");
            }

            return "1.0.0.0";
        }

        private async Task SaveConfigsToFileAsync()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_configStore, options);
                await File.WriteAllTextAsync(_configFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error guardando la configuración en el archivo: {ex.Message}");
                throw new InvalidOperationException("No se pudo guardar la configuración", ex);
            }
        }

        private void LoadConfigsFromFile()
        {
            try
            {
                if (File.Exists(_configFilePath))
                {
                    var json = File.ReadAllText(_configFilePath);
                    _configStore = JsonSerializer.Deserialize<Dictionary<string, object>>(json)
                                    ?? new Dictionary<string, object>();
                }
                else
                {
                    _configStore = new Dictionary<string, object>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando la configuración desde el archivo: {ex.Message}");
                _configStore = new Dictionary<string, object>();
            }

        }

    }
}
