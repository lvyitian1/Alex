using System.ComponentModel;

namespace RocketUI
{
	[TypeConverter(typeof(EnumTypeConverter<ScrollMode>))]
	public enum ScrollMode
	{
		Hidden,
		Auto,
		Visible
	}
}
