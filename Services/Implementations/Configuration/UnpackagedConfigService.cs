using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluentNotes.Services.Implementations.Configuration
{
    public class UnpackagedConfigService : IConfigurationService
    {
        private readonly string _configFilePath;
        private Dictionary<string, object> _configStore;

        public UnpackagedConfigService(string appName = AppPaths.AppName)
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
                {
                    if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }

                    return (T)value;
                }
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
                _configStore[key] = value;
                await SaveConfigsToFileAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error guardando la configuración para la clave '{key}': {ex.Message}");
            }
        }

        public async Task InitializeConfigsAsync()
        {
            if (!await IsFirstRunAsync())
                return;

            await SetConfigAsync(ConfigKeys.IsFirstRun, true);
            await SetConfigAsync(ConfigKeys.IsOnboardingCompleted, false);
            await SetConfigAsync(ConfigKeys.AppVersion, GetAppVersion());

            await SaveConfigsToFileAsync();
        }

        public async Task<bool> IsFirstRunAsync() =>
            await GetConfigAsync(ConfigKeys.IsFirstRun, true);

        private string GetAppVersion()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                return assembly.GetName().Version?.ToString() ?? "1.0.0";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo la versión de la app: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Error guardando la configuración en el archivo: {ex.Message}");
                throw new InvalidOperationException("No se pudo guardar la configuración", ex);
            }
        }

        private void LoadConfigsFromFile()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                {
                    _configStore = new Dictionary<string, object>();
                }
                else
                {
                    var json = File.ReadAllText(_configFilePath);
                    var rawConfigs = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json)
                                    ?? new Dictionary<string, JsonElement>();

                    _configStore = new Dictionary<string, object>();
                    foreach (var kvp in rawConfigs)
                    {
                        var element = kvp.Value;

                        object value = element.ValueKind switch
                        {
                            JsonValueKind.True or JsonValueKind.False => element.GetBoolean(),
                            JsonValueKind.Number => element.TryGetInt32(out int intVal) ? intVal :
                                                    element.TryGetInt64(out long longVal) ? longVal :
                                                    element.TryGetDouble(out double doubleVal) ? doubleVal :
                                                    element.GetDecimal(),
                            JsonValueKind.String when DateTime.TryParse(element.GetString(), out DateTime dateVal) => dateVal,
                            JsonValueKind.String => element.GetString() ?? string.Empty,
                            _ => element.ToString()
                        };

                        _configStore[kvp.Key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cargando la configuración desde el archivo: {ex.Message}");
                _configStore = new Dictionary<string, object>();
            }

        }

    }
}
