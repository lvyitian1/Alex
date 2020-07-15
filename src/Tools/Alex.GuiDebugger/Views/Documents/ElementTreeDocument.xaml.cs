using System;
using Alex.GuiDebugger.ViewModels.Documents;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Alex.GuiDebugger.Views.Documents
{
	public class ElementTreeDocument : ReactiveUserControl<ElementTreeDocumentViewModel>
	{
		public ElementTreeDocument()
		{
			AvaloniaXamlLoader.Load(this);
		}

		protected override void OnDataContextChanged(EventArgs e)
		{
			base.OnDataContextChanged(e);

			//UpdateView();

			//if (DataContext is ViewModels.Documents.ElementTreeDocument vm)
			//{
			//	vm.Properties.CollectionChanged += PropertiesOnCollectionChanged;
			//}
		}

		//private void PropertiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		//{
		//	UpdateView();
		//}

		//private void UpdateView()
		//{
		//	if (DataContext is ViewModels.Documents.ElementTreeDocument vm)
		//	{
		//		var dg = this.FindControl<DataGrid>("PropertiesDataGrid");
		//		dg.IsReadOnly = true;

		//		var collection = new DataGridCollectionView(vm.Properties);
		//		dg.Items = collection;
		//	}
		//}
	}
}