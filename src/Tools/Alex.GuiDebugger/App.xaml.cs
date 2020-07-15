using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Alex.GuiDebugger
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                desktopLifetime.MainWindow = new MainWindow()
                {
                    DataContext = new MainWindowViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
            {
                var mainView = new MainView()
                {
                    DataContext = new MainViewModel()
                };

                singleViewLifetime.MainView = mainView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}