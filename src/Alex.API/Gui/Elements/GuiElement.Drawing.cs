using Alex.API.Gui.Graphics;
using Alex.GuiDebugger.Common;
using Microsoft.Xna.Framework;
using RocketUI;

namespace Alex.API.Gui.Elements
{
    public partial class GuiElement
    {
        private float _rotation;
        [DebuggerVisible(Category = GuiDebuggerCategories.Layout)] public float Rotation
        {
            get => _rotation;
            set => _rotation = MathHelper.ToRadians(value);
        }

        [DebuggerVisible(Category = GuiDebuggerCategories.Layout)] public virtual Vector2 RotationOrigin { get; set; } = Vector2.Zero;

        [DebuggerVisible(Category = GuiDebuggerCategories.Layout)] public bool ClipToBounds { get; set; } = false;

        public GuiTexture2D Background;
        public GuiTexture2D BackgroundOverlay;


        protected virtual void OnDraw(GuiSpriteBatch graphics, GameTime gameTime)
        {
            graphics.FillRectangle(RenderBounds, Background);
            
            graphics.FillRectangle(RenderBounds, BackgroundOverlay);
        }
    }
}
