using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RocketUI
{
	public sealed class TextureBrush : Brush
	{

		public Texture2D Texture { get; }

		public TextureBrush(Texture2D texture)
		{
			Texture = texture;
		}

	}
}
