using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RocketUI;

namespace Alex.GuiDebugger.Models
{
	public class EditableGuiTexture
	{
		public Color?            Color           { get; set; }
		public string			 TextureResource { get; set; }
		//public ITexture2D        Texture         { get; set; }
		public TextureRepeatMode RepeatMode      { get; set; }
		public Color?            Mask            { get; set; }
		public Vector2?          Scale           { get; set; }

		public EditableGuiTexture()
		{

		}


	}
}
