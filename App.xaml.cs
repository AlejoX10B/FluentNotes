using FluentNotes.Services.Implementations;
using FluentNotes.Services.Interfaces;
using Microsoft.UI.Xaml;

namespace FluentNotes
{

    public partial class App : Application
    {
        private IConfigurationService _configService;
        private Window? _window;

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            _configService = ConfigurationServiceFactory.CreateConfigService();
            await _configService.InitializeConfigsAsync();

            bool isFirstRun = await _configService.IsFirstRunAsync();
            System.Diagnostics.Debug.WriteLine($"Is First Run: {isFirstRun}");

            _window = new MainWindow();
            _window.Activate();
        }
    }
}
