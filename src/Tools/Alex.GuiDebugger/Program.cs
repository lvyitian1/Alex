using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;

namespace Alex.GuiDebugger
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder
                .Configure<App>()
                .With(new X11PlatformOptions()
                {
                    EnableMultiTouch = true
                })
                .With(new Win32PlatformOptions()
                {
                    EnableMultitouch = true,
                    AllowEglInitialization = true
                })
                .UseReactiveUI()
                .UsePlatformDetect()
                .LogToDebug()
        ;
    }
}
