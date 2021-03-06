using System;
using Alex.API.Graphics.Typography;
using Alex.API.Gui;
using Alex.API.Gui.Elements;
using Alex.API.Gui.Elements.Controls;
using Alex.API.Gui.Elements.Layout;
using Alex.API.Input;
using Alex.API.Utils;
using Microsoft.Xna.Framework;
using MiNET.Net;
using MiNET.UI;

namespace Alex.Gui.Forms
{
    public class SimpleFormDialog : FormBase
    {
        private GuiStackContainer      Header { get; }
        //public  GuiMultiStackContainer Footer { get; }
        
        private GuiStackMenu           StackMenu { get; }
        public SimpleFormDialog(uint formId, BedrockFormManager parent, SimpleForm form, InputManager inputManager) : base(formId, parent, inputManager)
        {
            StackMenu = new GuiStackMenu();
            StackMenu.Anchor = Alignment.Fill;
            StackMenu.ChildAnchor = Alignment.MiddleCenter;
            StackMenu.Background = Color.Black * 0.35f;

            if (!string.IsNullOrWhiteSpace(form.Content))
            {
                StackMenu.AddLabel(FixContrast(form.Content));
                StackMenu.AddSpacer();
            }

            var btns = form.Buttons.ToArray();
            for (var index = 0; index < btns.Length; index++)
            {
                var button = btns[index];
                int idx = index;

                Action submitAction = () =>
                {
                    var packet = McpeModalFormResponse.CreateObject();
                    packet.formId = formId;
                    packet.data = idx.ToString();
                    //JsonConvert.SerializeObject(idx)
                    parent.SendResponse(packet);
                    
                    parent.Hide(formId);
                };
                
                if (button.Image != null)
                {
                    switch (button.Image.Type)
                    {
                        case "url":
                            StackMenu.AddChild(new FormImageButton(button.Image.Url, button.Text, submitAction));
                            continue;
                            break;
                        case "path":
                            break;
                    }
                }
                
                var item = StackMenu.AddMenuItem(button.Text, submitAction);
            }
            
            Background = Color.Transparent;

            var width  = 356;
            var height = width;
			
            ContentContainer.Width = ContentContainer.MinWidth = ContentContainer.MaxWidth = width;
            ContentContainer.Height = ContentContainer.MinHeight = ContentContainer.MaxHeight = height;
            
            SetFixedSize(width, height);
            
            ContentContainer.AutoSizeMode = AutoSizeMode.None;
			
            Container.Anchor = Alignment.MiddleCenter;

            var bodyWrapper = new GuiContainer();
            bodyWrapper.Anchor = Alignment.Fill;
            bodyWrapper.Padding = new Thickness(5, 0);
            bodyWrapper.AddChild(StackMenu);
            
            Container.AddChild(bodyWrapper);
            
            Container.AddChild(Header = new GuiStackContainer()
            {
                Anchor = Alignment.TopFill,
                ChildAnchor = Alignment.BottomCenter,
                Height = 32,
                Padding = new Thickness(3),
                Background = Color.Black * 0.5f
            });
            
            Header.AddChild(new GuiTextElement()
            {
                Text      = FixContrast(form.Title),
                TextColor = TextColor.White,
                Scale     = 2f,
                FontStyle = FontStyle.DropShadow,
                
                Anchor = Alignment.BottomCenter,
            });
			
            StackMenu.Margin = new Thickness(0, Header.Height, 0, 0);
        }
    }
}