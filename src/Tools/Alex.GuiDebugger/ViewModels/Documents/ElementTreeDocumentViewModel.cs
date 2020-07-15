﻿using System;
 using System.Collections.ObjectModel;
using System.Reactive;
 using System.Reactive.Disposables;
 using System.Reactive.Linq;
 using System.Threading.Tasks;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
 using Avalonia.Controls.Primitives;
 using DynamicData;
 using DynamicData.Binding;
 using ReactiveUI;

namespace Alex.GuiDebugger.ViewModels.Documents
{
    public class ElementTreeDocumentViewModel : ViewModelBase, IActivatableViewModel
    {
        private ElementTreeItem _elementTreeItem;
        public ViewModelActivator Activator { get; }
        public string Title { get; set; }

        public ElementTreeItem ElementTreeItem
        {
            get => _elementTreeItem;
            set => this.RaiseAndSetIfChanged(ref _elementTreeItem, value);
        }
        
        public ObservableCollection<ElementTreeItemProperty> Properties { get; }

        public ReactiveCommand<Unit, Unit> RefreshPropertiesCommand { get; }

        public ElementTreeDocumentViewModel(ElementTreeItem elementTreeItem = null)
        {
            Activator = new ViewModelActivator();
            ElementTreeItem = elementTreeItem ?? new ElementTreeItem();
            Title = ElementTreeItem.ElementType;

            RefreshPropertiesCommand = ReactiveCommand.CreateFromTask(RefreshProperties);
            Properties = new ObservableCollection<ElementTreeItemProperty>();
            
            this.WhenActivated(disposables =>
            {
                /* handle activation */
                this.WhenAnyValue(model => model.ElementTreeItem)
                    .Throttle(TimeSpan.FromSeconds(2))
                    // .Select(eti =>
                    //     AlexGuiDebuggerInteraction.Instance.GetElementTreeItemProperties(eti.Id).GetAwaiter().GetResult())
                    // .Subscribe(collection =>
                    // {
                    //     Properties.Clear();
                    //     Properties.AddRange(collection);
                    // })
                    .InvokeCommand(RefreshPropertiesCommand)
                    .DisposeWith(disposables);
                
                if (ElementTreeItem != null && ElementTreeItem.Id != default)
                {
                    RefreshProperties();
                }

                Disposable
                    .Create(() => { /* handle deactivation */ })
                    .DisposeWith(disposables);
            });
        }

        private async Task RefreshProperties()
        {
            var newItems = await AlexGuiDebuggerInteraction.Instance.GetElementTreeItemProperties(ElementTreeItem.Id);
            Properties.Clear();
            Properties.AddRange(newItems);
        }

        
        // public override void OnSelected()
        // {
        //     base.OnSelected();
        //     AlexGuiDebuggerInteraction.Instance.HighlightElement(ElementTreeItem.Id);
        // }
    }
}
