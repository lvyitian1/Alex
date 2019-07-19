using Microsoft.Xna.Framework;

namespace Alex.GuiDebugger.Models
{
	public class EditableColor 
	{
		public Color Color { get; set; }

		public EditableColor()
		{

		}

		public EditableColor(Color color)
		{
			Color = color;
		}

		public static implicit operator EditableColor(Color color)
		{
			return new EditableColor(color);
		}

		public static implicit operator Color(EditableColor color)
		{
			return color.Color;
		}
	}
}
