﻿using System.Collections.ObjectModel;
using System.Reactive;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
using DynamicData;
using ReactiveUI;

namespace Alex.GuiDebugger.ViewModels.Tools
{
    public class ElementTreeToolViewModel : ViewModelBase
    {
        public ObservableCollection<ElementTreeItem> ElementTreeItems { get; }

        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public ElementTreeToolViewModel()
        {
            ElementTreeItems = new ObservableCollection<ElementTreeItem>();
            RefreshCommand = ReactiveCommand.Create(Refresh);
            //Refresh();
        }


        private void Refresh()
        {
            var newItems = AlexGuiDebuggerInteraction.Instance.GetElementTreeItems().Result;
            ElementTreeItems.Clear();
            ElementTreeItems.AddRange(newItems);
        }
    }
}
