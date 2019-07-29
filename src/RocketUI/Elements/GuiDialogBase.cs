namespace RocketUI
{
    public abstract class GuiDialogBase : GuiScreen
    {

        public bool ShowBackdrop { get; set; }

        protected GuiContainer ContentContainer;


        protected GuiDialogBase()
        {
            AddChild(ContentContainer = new GuiContainer()
            {
                Anchor = Alignment.MiddleCenter
            });
        }



    }
}
