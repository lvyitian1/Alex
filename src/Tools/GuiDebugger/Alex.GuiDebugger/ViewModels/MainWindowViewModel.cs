using Dock.Model;
using Dock.Serializer;
using ReactiveUI;

namespace Alex.GuiDebugger.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private IDockSerializer _serializer;
        private IFactory _factory;
        private IDockable        _layout;
        private string       _currentView;

        public IDockSerializer Serializer
        {
            get => _serializer;
            set => this.RaiseAndSetIfChanged(ref _serializer, value);
        }
        
        public IFactory Factory
        {
            get => _factory;
            set => this.RaiseAndSetIfChanged(ref _factory, value);
        }

        public IDockable Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }

        public string CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }
    }
}
