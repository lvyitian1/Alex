namespace RocketUI
{
    public interface IFocusableElement : IGuiElement
    {
        [DebuggerVisible] bool Focused { get; set; }
    }
}
