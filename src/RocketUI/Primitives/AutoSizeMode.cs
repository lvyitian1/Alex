using System.ComponentModel;

namespace RocketUI
{
    [TypeConverter(typeof(EnumTypeConverter<AutoSizeMode>))]
    public enum AutoSizeMode
    {
        None,
        GrowAndShrink,
        GrowOnly,
    }

}
