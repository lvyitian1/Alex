using System;
using System.Collections.ObjectModel;
using Alex.GuiDebugger.Factories;
using Alex.GuiDebugger.Models;
using Avalonia;
using Avalonia.Logging.Serilog;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.Views;
using Avalonia.ReactiveUI;
using Dock.Model;
using Dock.Serializer;

namespace Alex.GuiDebugger
{
    class Program
    {
        private static void Print(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Print(ex.InnerException);
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

                //serializer.Save(path, vm.Layout);
            }
            catch (Exception ex)
            {
                Print(ex);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UseReactiveUI()
                //.UseDataGrid()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
