using System.ComponentModel;

namespace RocketUI
{
    [TypeConverter(typeof(EnumTypeConverter<TextureRepeatMode>))]
    public enum TextureRepeatMode
    {
        NoRepeat,
        Tile,
        ScaleToFit,
        Stretch,
        NoScaleCenterSlice
    }
}
