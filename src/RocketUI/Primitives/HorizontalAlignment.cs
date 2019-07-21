using System.ComponentModel;

namespace RocketUI
{
	[TypeConverter(typeof(EnumTypeConverter<HorizontalAlignment>))]
    public enum HorizontalAlignment
	{
		None		= Alignment.None,

		Left		= Alignment.MinX,
		Center		= Alignment.CenterX,
		Right		= Alignment.MaxX,

		FillParent	= Alignment.FillX,
	}
}
