using System;
using System.Collections.Generic;
using System.Text;
using Catel.Data;
using RocketUI;

namespace Alex.GuiDebugger.Models
{
	public class EditableAlignment : ModelBase
	{
		public Alignment HorizontalAlignment
		{
			get { return _alignment & Alignment.OrientationX; }
			set { _alignment = (_alignment & Alignment.OrientationY) | (value & Alignment.OrientationX); }
		}

		public Alignment VerticalAlignment
		{
			get { return _alignment & Alignment.OrientationY; }
			set { _alignment = (_alignment & Alignment.OrientationX) | (value & Alignment.OrientationY); }
		}


		public bool IsHorizontalNone
		{
			get { return HorizontalAlignment == Alignment.NoneX; }
			set
			{
				if (value) HorizontalAlignment = Alignment.NoneX;
			}
		}

		public bool IsHorizontalLeft
		{
			get { return HorizontalAlignment == Alignment.MinX; }
			set
			{
				if (value) HorizontalAlignment = Alignment.MinX;
			}
		}

		public bool IsHorizontalRight
		{
			get { return HorizontalAlignment == Alignment.MaxX; }
			set
			{
				if (value) HorizontalAlignment = Alignment.MaxX;
			}
		}

		public bool IsHorizontalCenter
		{
			get { return HorizontalAlignment == Alignment.CenterX; }
			set
			{
				if (value) HorizontalAlignment = Alignment.CenterX;
			}
		}

		public bool IsHorizontalFill
		{
			get { return HorizontalAlignment == Alignment.FillX; }
			set
			{
				if (value) HorizontalAlignment = Alignment.FillX;
			}
		}


		public bool IsVerticalNone
		{
			get { return VerticalAlignment == Alignment.NoneY; }
			set
			{
				if (value) VerticalAlignment = Alignment.NoneY;
			}
		}

		public bool IsVerticalTop
		{
			get { return VerticalAlignment == Alignment.MinY; }
			set
			{
				if (value) VerticalAlignment = Alignment.MinY;
			}
		}

		public bool IsVerticalBottom
		{
			get { return VerticalAlignment == Alignment.MaxY; }
			set
			{
				if (value) VerticalAlignment = Alignment.MaxY;
			}
		}

		public bool IsVerticalCenter
		{
			get { return VerticalAlignment == Alignment.CenterY; }
			set
			{
				if (value) VerticalAlignment = Alignment.CenterY;
			}
		}

		public bool IsVerticalFill
		{
			get { return VerticalAlignment == Alignment.FillY; }
			set
			{
				if (value) VerticalAlignment = Alignment.FillY;
			}
		}

		private Alignment _alignment;

		public Alignment Alignment
		{
			get => _alignment;
		}

		public EditableAlignment() : this(Alignment.Default)
		{
		}

		public EditableAlignment(Alignment alignment)
		{
			_alignment = alignment;
		}


		public static implicit operator Alignment(EditableAlignment alignment)
		{
			return alignment.Alignment;
		}

		public static implicit operator EditableAlignment(Alignment alignment)
		{
			return new EditableAlignment(alignment);
		}
	}
}