using FluentNotes.Services.Implementations.Configuration;
using Microsoft.UI.Xaml;
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
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                _services = AppServicesFactory.CreateServices();
                await _services.ConfigurationService.InitializeConfigsAsync();
                await _services.DirectoryService.InitializeDirectoriesAsync();
                await _services.DatabaseService.InitializeDatabaseAsync();

                _window = new MainWindow(_services);
                _window.Activate();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error durante el inicio de la app: {ex.Message}");
                throw;
            }
        }
    }
}
