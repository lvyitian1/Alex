using System.ComponentModel;

namespace RocketUI
{
	[TypeConverter(typeof(EnumTypeConverter<Orientation>))]
	public enum Orientation
	{
		Vertical,
		Horizontal
	}
}
