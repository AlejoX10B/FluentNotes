using FluentNotes.Services;
using FluentNotes.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;

namespace FluentNotes
{

    public partial class App : Application
    {
        private static IHost? _host;

        public static IServiceProvider Services => _host?.Services ?? 
            throw new InvalidOperationException("Servicio no inicializado");

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _host = CreateHostBuilder().Build();
            
            AppSetup.Initialize();

            var window = Services.GetRequiredService<MainWindow>();
            window.Activate();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IDataService, MockDataService>();
                    services.AddTransient<MainWindow>();
                });
        }
    }
}
