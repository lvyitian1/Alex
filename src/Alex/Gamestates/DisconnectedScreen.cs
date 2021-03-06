﻿using Alex.API.GameStates;
using Alex.API.Gui;
using Alex.API.Gui.Elements;
using Alex.API.Gui.Elements.Controls;
using Alex.API.Utils;
using Alex.Gamestates.Common;

namespace Alex.Gamestates
{
    public class DisconnectedScreen : GuiMenuStateBase
    {
	    private static string _reason = null;

	    public static string DisconnectReason
	    {
		    get
		    {
			    return _reason;
		    }
		    set
		    {
			    _reason = value;
				if (_activeScreen != null && !string.IsNullOrWhiteSpace(value))
				{
					_activeScreen.DisconnectedTextElement.Text = _reason;
				}
		    }
	    }

	    private static DisconnectedScreen _activeScreen = null;
	    public         string             Reason                  { get; set; } = "disconnect.lost";
	    public         GuiTextElement     DisconnectedTextElement { get; private set; }
	    public DisconnectedScreen()
		{
			TitleTranslationKey = "multiplayer.disconnect.generic";

			Body.ChildAnchor = Alignment.MiddleCenter;
			Body.AddChild(DisconnectedTextElement = new GuiTextElement()
			{
				Text = Reason,
				TextColor = TextColor.Red,
				Anchor = Alignment.MiddleCenter
			});

			Footer.AddChild(new GuiButton(MenuButtonClicked)
			{
				TranslationKey = "gui.toTitle",
				Anchor = Alignment.MiddleCenter,
				Modern = false
			});
		}

		private void MenuButtonClicked()
		{
			//if (ParentState != null)
			//{
			//	Alex.GameStateManager.SetActiveState(ParentState);
			//}
			//else
			//{
			Alex.GameStateManager.RemoveState("play");
			Alex.GameStateManager.SetActiveState<TitleState>("title");
			Alex.GameStateManager.RemoveState(this);
			//}

			Alex.IsMouseVisible = true;
		}

	    protected override void OnShow()
	    {
		    Alex.IsMouseVisible = true;
		    _activeScreen = this;

		    base.OnShow();
	    }

	    /// <inheritdoc />
	    protected override void OnHide()
	    {
		    _activeScreen = null;
		    DisconnectReason = null;
		    base.OnHide();
	    }
    }
}
