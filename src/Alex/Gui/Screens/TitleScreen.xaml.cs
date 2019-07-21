using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alex.API.Gui;
using Alex.API.Gui.Elements.Layout;
using Alex.GameStates.Gui.Common;
using RocketUI.Serialization.Xaml;


namespace Alex.Gui.Screens
{
	public partial class TitleScreen : GuiGameStateBase
	{
		public TitleScreen()
		{
			RocketXamlLoader.Load(this);
		}
	}
}