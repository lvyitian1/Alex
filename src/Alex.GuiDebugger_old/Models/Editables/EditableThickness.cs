using System;
using System.Collections.Generic;
using System.Text;
using RocketUI;

namespace Alex.GuiDebugger.Models
{
	public class EditableThickness
	{
		public int Left   { get; set; }
		public int Top    { get; set; }
		public int Right  { get; set; }
		public int Bottom { get; set; }
		
		public EditableThickness()
		{

		}

		public EditableThickness(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public EditableThickness(Thickness thickness) : this(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom)
		{

		}

		public static implicit operator EditableThickness(Thickness thickness)
		{
			return new EditableThickness(thickness);
		}
		public static implicit operator Thickness(EditableThickness thickness)
		{
			return new Thickness()
			{
				Left = thickness.Left,
				Top = thickness.Top,
				Bottom = thickness.Bottom,
				Right = thickness.Right
			};
		}

	}
}
