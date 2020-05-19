using Dock.Model;
using Dock.Model.Controls;


namespace Alex.GuiDebugger.ViewModels
{
    public class MainView : DockBase
    {
        public override IDockable? Clone()
        {
            var mainViewModel = new MainView();
            CloneHelper.CloneDockProperties(this, mainViewModel);
            
            return mainViewModel;
        }
    }
}
