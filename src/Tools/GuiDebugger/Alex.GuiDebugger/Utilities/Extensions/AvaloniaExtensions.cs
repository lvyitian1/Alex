using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Alex.GuiDebugger.Utilities.Extensions
{
    public static class AvaloniaExtensions
    {

        public static Window GetMainWindow(this Application application)
        {
            if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                return desktopLifetime.MainWindow;

            if (application.ApplicationLifetime is IControlledApplicationLifetime controlledApplicationLifetime)
                return null;
            
            if (application.ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
                return null;

            return null;
        }
        
    }
}