using Microsoft.Xna.Framework;

namespace RocketUI
{
    public interface ICursorInputListener : IInputListener
    {
        Vector2 GetCursorPositionDelta();
        Vector2 GetCursorPosition();
    }
}
