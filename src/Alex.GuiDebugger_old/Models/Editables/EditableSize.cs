using System;
using System.Collections.Generic;
using System.Text;
using RocketUI;

namespace Alex.GuiDebugger.Models
{
	public class EditableSize
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public EditableSize()
		{

		}

		public EditableSize(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public static implicit operator EditableSize(Size size)
		{
			return new EditableSize(size.Width, size.Height);
		}

		public static implicit operator Size(EditableSize size)
		{
			return new Size
			{
				Width = size.Width,
				Height = size.Height
			};
		}

	}
}
