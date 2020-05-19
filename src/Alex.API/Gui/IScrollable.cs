using RocketUI;

namespace Alex.API.Gui
{
    public interface IScrollable
    {
        bool CanScroll(Orientation orientation);
        
        void InvokeScroll(Orientation orientation, int delta);
    }
}