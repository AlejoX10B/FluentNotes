using FluentNotes.Services.Implementations.Configuration;
using FluentNotes.Utils.Logging;
using FluentNotes.Utils.Providers;
using Microsoft.UI.Xaml;
using Serilog;
using System;

namespace FluentNotes
{

    public partial class App : Application
    {
        private AppServices _services;
        private Window? _window;

        public App()
        {
            InitializeComponent();
            this.UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            try
            {
                Log.Fatal(e.Exception, "Excepción no controlada de la aplicación");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine($"Excepción no controlada: {e.Exception.Message}");
            }
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                Log.Information("--- Iniciando Aplicación ---");

                _services = AppServicesFactory.CreateServices();
                await _services.ConfigurationService.InitializeConfigsAsync();
                await _services.DirectoryService.InitializeDirectoriesAsync();
#if DEBUG
                await LoggerConfig.ConfigureLoggerAsync(_services.DirectoryService);
                Log.Information("Sistema de logging configurado correctamente");
                Log.Information("Packaged App: {IsPackaged}", ApplicationTypeDetector.IsPackagedApp());
#endif
                await _services.DatabaseService.InitializeDatabaseAsync();

                _window = new MainWindow(_services);
                _window.Activate();

                Log.Information("--- Aplicación iniciada correctamente ---");
            }
            catch (Exception ex)
            {
                if (Log.Logger != null)
                {
                    Log.Fatal(ex, "Error crítico durante el inicio de la aplicación");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Error crítico durante el inicio de la app: {ex.Message}");
                }

                throw;
            }
        }

        public void ShutdownApp()
        {
            try
            {
                Log.Information("--- Cerrando Aplicación ---");
                Log.Information("--- Aplicación cerrada correctamente ---");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error crítico durante el cierre de la aplicación");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
