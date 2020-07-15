using ReactiveUI;

namespace Alex.GuiDebugger.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; } = new MainViewModel();

        public MainWindowViewModel()
        {
            
        }
    }
}
