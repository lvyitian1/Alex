using System.Collections.ObjectModel;
using System.Linq;
using Alex.GuiDebugger.Factories;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.ViewModels.Tools;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Dock.Avalonia;
using Dock.Avalonia.Controls;
using Dock.CodeGen;
using Dock.Model;
using Dock.Model.Controls;
using Dock.Serializer;

namespace Alex.GuiDebugger.Views
{
	public class MainWindow : MetroWindow
	{
		private DockSerializer _serializer = new DockSerializer(typeof(ObservableCollection<>));

		public MainWindow()
		{
			this.InitializeComponent();
#if DEBUG
			this.AttachDevTools();
#endif
			this.Closing += (sender, e) =>
			{
				if (this.DataContext is MainWindowViewModel vm)
				{
					if (vm.Layout is IDock dock)
					{
						dock.Close();
					}
				};
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
