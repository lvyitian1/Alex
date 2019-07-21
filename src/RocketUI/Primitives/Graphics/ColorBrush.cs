using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RocketUI
{
	public sealed class ColorBrush : Brush
	{
		public Color Color { get; }

		public ColorBrush(Color color)
		{
			Color = color;
		}

	}
}
