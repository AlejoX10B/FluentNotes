using FluentNotes.Services.Implementations.Configuration;
using FluentNotes.Utils.Constants;
using FluentNotes.Views.Dialogs;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace FluentNotes
{

    public sealed partial class MainWindow : Window
    {
        private readonly AppServices _services;

        public MainWindow(AppServices appServices)
        {
            _services = appServices;

            InitializeComponent();
            ConfigTitleBar();
        }

        private void ConfigTitleBar()
        {
            ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.PreferredTheme = TitleBarTheme.UseDefaultAppMode;
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            //SetTitleBar(AppTitleBar);
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool isFirstRun = await _services.ConfigurationService.IsFirstRunAsync();
            if (!isFirstRun)
                return;

            var dialog = new OnboardingDialog { XamlRoot = Content.XamlRoot };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                await _services.ConfigurationService.SetConfigAsync(ConfigKeys.IsOnboardingCompleted, true);
                Closed += MainWindow_Closed;
            }
        }

        private async void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            await _services.ConfigurationService.SetConfigAsync(ConfigKeys.IsFirstRun, false);
            Closed -= MainWindow_Closed;
        }

    }
}
