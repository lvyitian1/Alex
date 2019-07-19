using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using RocketUI;

namespace Alex.GuiDebugger.Models
{
	public class EditableRectangle
	{
		public int X   { get; set; }
		public int Y    { get; set; }
		public int Width  { get; set; }
		public int Height { get; set; }
		
		public EditableRectangle()
		{

		}

		public EditableRectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}


		public static implicit operator EditableRectangle(Rectangle rectangle)
		{
			return new EditableRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
		public static implicit operator Rectangle(EditableRectangle rectangle)
		{
			return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

	}
}
