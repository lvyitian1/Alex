using Alex.GuiDebugger.ViewModels.Documents;
using Alex.GuiDebugger.ViewModels.Tools;

namespace Alex.GuiDebugger.ViewModels
{
    public class MainViewModel :ViewModelBase
    {
        public ElementTreeDocumentViewModel ElementTreeDocumentViewModel { get; set; } = new ElementTreeDocumentViewModel();
        public ElementTreeToolViewModel ElementTreeToolViewModel { get; set; } = new ElementTreeToolViewModel();

        public MainViewModel()
        {
            
        }
    }
}
