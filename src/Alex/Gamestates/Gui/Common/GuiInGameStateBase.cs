using Alex.API.Graphics;
using Alex.API.Gui.Graphics;
using Microsoft.Xna.Framework;

namespace Alex.GameStates.Gui.Common
{
    public class GuiInGameStateBase : GuiMenuStateBase
    {

        public GuiInGameStateBase()
        {
            Background = GuiTexture2D.Empty;
            BackgroundOverlay = new Color(Color.Black, 0.65f);
        }
        
        protected override void OnDraw(IRenderArgs args)
        {
            ParentState.Draw(args);
        }

        protected override void OnShow()
        {
            Alex.IsMouseVisible = true;
            base.OnShow();
        }
    }
}
