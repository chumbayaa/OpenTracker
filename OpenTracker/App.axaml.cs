﻿using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using Avalonia.ThemeManager;
using OpenTracker.Interfaces;
using OpenTracker.Utils;
using OpenTracker.ViewModels;
using OpenTracker.Views;
using System.IO;

namespace OpenTracker
{
    public class App : Application
    {
        public static IThemeSelector? Selector { get; private set; }
        public static IDialogService? DialogService { get; private set; }

        private static void CopyDefaultThemesToAppData()
        {
            var themePath = AppPath.AppDataThemesPath;

            if (!Directory.Exists(themePath))
            {
                Directory.CreateDirectory(themePath);
            }

            foreach (var srcTheme in Directory.GetFiles(AppPath.AppRootThemesPath))
            {
                var filename = Path.GetFileName(srcTheme);
                var destTheme = Path.Combine(themePath, filename);

                if (File.Exists(destTheme))
                {
                    File.Delete(destTheme);
                }

                File.Copy(srcTheme, destTheme);
            }
        }

        private static void MakeDefaultThemeFirst()
        {
            foreach (var theme in Selector!.Themes!)
            {
                if (theme.Name == "Default")
                {
                    Selector.Themes.Remove(theme);
                    Selector.Themes.Insert(0, theme);
                    break;
                }
            }
        }

        private void InitializeThemes()
        {
            CopyDefaultThemesToAppData();
            Selector = ThemeSelector.Create(AppPath.AppDataThemesPath, this);
            MakeDefaultThemeFirst();
        }

        private static void SetThemeToLastOrDefault()
        {
            var lastThemeFilePath = AppPath.LastThemeFilePath;

            if (File.Exists(lastThemeFilePath))
            {
                Selector!.LoadSelectedTheme(lastThemeFilePath);
            }
            else
            {
                Selector!.ApplyTheme(Selector!.Themes![0]);
            }
        }

        private static void InitializeDialogService(Window owner)
        {
            DialogService = new DialogService(owner);
            DialogService.Register<MessageBoxDialogVM, MessageBoxDialog>();
            DialogService.Register<AboutDialogVM, AboutDialog>();
            DialogService.Register<ErrorBoxDialogVM, ErrorBoxDialog>();
        }

        public override void Initialize()
        {
            Logger.Sink = new AvaloniaSerilogSink(
                AppPath.AvaloniaLogFilePath, Serilog.Events.LogEventLevel.Warning);

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                InitializeThemes();

                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;

                using var scope = ContainerConfig.Configure().BeginLifetimeScope();
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = scope.Resolve<IMainWindowVM>()
                };
                
                SetThemeToLastOrDefault();
                InitializeDialogService(desktop.MainWindow);

                desktop.Exit += (sender, e) => Selector!.SaveSelectedTheme(
                    AppPath.LastThemeFilePath);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
