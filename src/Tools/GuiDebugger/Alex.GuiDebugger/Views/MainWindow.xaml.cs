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
using Dock.Model;
using Dock.Model.Controls;
using Dock.Serializer;

namespace Alex.GuiDebugger.Views
{
	public class MainWindow : MetroWindow
	{
		private IDockSerializer _serializer = new DockSerializer(typeof(ObservableCollection<>));

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
			this.FindControl<MenuItem>("FileNew").Click += (sender, e) =>
			{
				if (this.DataContext is MainWindowViewModel vm)
				{
					if (vm.Layout is IDock root)
					{
						root.Close();
					}
					vm.Factory = new DefaultDockFactory(new DockData());
					vm.Layout = vm.Factory.CreateLayout();
					vm.Factory.InitLayout(vm.Layout);
				}
			};

			this.FindControl<MenuItem>("FileOpen").Click += async (sender, e) =>
			{
				var dlg = new OpenFileDialog();
				dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
				dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
				var result = await dlg.ShowAsync(this);
				if (result != null)
				{
					if (this.DataContext is MainWindowViewModel vm)
					{
						IDock layout = _serializer.Load<RootDock>(result.FirstOrDefault());
						if (vm.Layout is IDock root)
						{
							root.Close();
						}
						vm.Layout = layout;
						vm.Factory.InitLayout(vm.Layout);
					}
				}
			};

			this.FindControl<MenuItem>("FileSaveAs").Click += async (sender, e) =>
			{
				var dlg = new SaveFileDialog();
				dlg.Filters.Add(new FileDialogFilter() { Name = "Json", Extensions = { "json" } });
				dlg.Filters.Add(new FileDialogFilter() { Name = "All", Extensions = { "*" } });
				dlg.InitialFileName = "Layout";
				dlg.DefaultExtension = "json";
				var result = await dlg.ShowAsync(this);
				if (result != null)
				{
					if (this.DataContext is MainWindowViewModel vm)
					{
						_serializer.Save(result, vm.Layout);
					}
				}
			};
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
