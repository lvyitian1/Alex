using Dock.Model;
using Dock.Model.Controls;

namespace Alex.GuiDebugger.ViewModels
{
    public class MainView : RootDock
    {
        public override IDockable? Clone()
        { 
            var mainView = new MainView();

            CloneHelper.CloneDockProperties(this, mainView);
            CloneHelper.CloneRootDockProperties(this, mainView);

            return mainView;
        }
    }
}
