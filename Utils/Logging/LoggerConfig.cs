using FluentNotes.Services.Interfaces;
using FluentNotes.Utils.Constants;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNotes.Utils.Logging
{
    internal class LoggerConfig
    {
        public static async Task ConfigureLoggerAsync(IDirectoryService directoryService)
        {
            try
            {
                var appDataPath = await directoryService.GetAppDataDirectoryAsync();
                var logsPath = Path.Combine(appDataPath, AppPaths.LogsFolder);

                Directory.CreateDirectory(logsPath);

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithMachineName()
                    .Enrich.WithProcessId()
                    .Enrich.WithProcessName()
                    .Enrich.WithThreadId()
                    .Enrich.WithProperty("Application", AppPaths.AppName)
                    .WriteTo.File(
                        path: Path.Combine(logsPath, "app-.log"),
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 50 * 1024 * 1024,
                        rollOnFileSizeLimit: true,
                        retainedFileCountLimit: 30,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u4}] [{SourceContext:l}] [{ThreadId}] {Message:lj}{NewLine}{Exception}",
                        encoding: Encoding.UTF8,
                        buffered: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(10),
                        restrictedToMinimumLevel: LogEventLevel.Information
                    )
#if DEBUG
                    .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Debug)
                        .WriteTo.File(
                            path: Path.Combine(logsPath, "debug-.log"),
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7,
                            fileSizeLimitBytes: 100 * 1024 * 1024,
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u3}] [{SourceContext:l}] [{ThreadId}] {Message:lj}{NewLine}{Exception}"
                        )
                    )
#endif
                    .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(evt => evt.Level >= LogEventLevel.Warning)
                        .WriteTo.File(
                            path: Path.Combine(logsPath, "errors-.log"),
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 90,
                            fileSizeLimitBytes: 10 * 1024 * 1024,
                            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level:u4}] {Message:lj}{NewLine}{Exception}{NewLine}---{NewLine}"
                        )
                    )
#if DEBUG
                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                        restrictedToMinimumLevel: LogEventLevel.Debug
                    )
                    .WriteTo.Debug(
                        outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}",
                        restrictedToMinimumLevel: LogEventLevel.Debug
                    )
#endif
                    .CreateLogger();

                Log.Information("---- {AppName} Logging Iniciado ----", AppPaths.AppName);
                Log.Information("Máquina: {MachineName}, Proceso: {ProcessName} (PID: {ProcessId})",
                    Environment.MachineName, Environment.ProcessPath, Environment.ProcessId);

#if DEBUG
                Log.Debug("Logger configurado en modo DEBUG - Logs verbosos habilitados");
#else
                Log.Information("Logger configurado en modo RELEASE - Logs de producción habilitados");
#endif

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al configurar el logger: {ex.Message}");

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Debug()
                    .CreateLogger();

                Log.Warning("Logger iniciado en modo fallback debido a error de configuración");
            }
        }
    }
}
