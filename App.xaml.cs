using FluentNotes.Services.Implementations.Configuration;
using FluentNotes.Services.Interfaces;
using Microsoft.UI.Xaml;

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
            _services = AppServicesFactory.CreateServices();
            await _services.ConfigurationService.InitializeConfigsAsync();
            await _services.DirectoryService.InitializeDirectoriesAsync();

            bool isFirstRun = await _services.ConfigurationService.IsFirstRunAsync();
            System.Diagnostics.Debug.WriteLine($"Is First Run: {isFirstRun}");

            _window = new MainWindow();
            _window.Activate();
        }
    }
}
