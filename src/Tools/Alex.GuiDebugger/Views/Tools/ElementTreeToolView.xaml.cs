using System.Linq;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
using Alex.GuiDebugger.Utilities.Extensions;
using Alex.GuiDebugger.ViewModels;
using Alex.GuiDebugger.ViewModels.Documents;
using Alex.GuiDebugger.ViewModels.Tools;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Alex.GuiDebugger.Views.Tools
{
    public class ElementTreeToolView : ReactiveUserControl<ElementTreeToolViewModel>
    {
        public ElementTreeToolView()
        {
            AvaloniaXamlLoader.Load(this);

            this.FindControl<Button>("RefreshButton").Click += async (sender, e) =>
            {
                    var srv   = AlexGuiDebuggerInteraction.Instance;
                    var items = await srv.GetElementTreeItems();

                    if (items != null)
                    {
                        if (DataContext is ElementTreeToolViewModel vm)
                        {
                            vm.ElementTreeItems.Clear();

                            foreach (var item in items)
                            {
                                vm.ElementTreeItems.Add(item);
                            }
                        }
                    }
            };

            this.FindControl<TreeView>("TreeView").SelectionChanged += async (sender, e) =>
            {
                if (!(Application.Current is App app))
                    return;

                if (!(app.GetMainWindow().DataContext is MainWindowViewModel mainWindowViewModel))
                    return;


                var selectedItem = ((sender as TreeView).SelectedItem as ElementTreeItem);
                if (selectedItem == null) return;

                mainWindowViewModel.MainViewModel.ElementTreeDocumentViewModel.ElementTreeItem = selectedItem;
            };

        }
    }
}
