using System.ComponentModel;
using System.Globalization;

namespace XTI_Core;

public sealed class TextValueTypeConverter<T> : TypeConverter
    where T : TextValue
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str)
        {
            var typeToConvert = typeof(T);
            var ctor = typeToConvert.GetConstructor(new[] { typeof(string) });
            if (ctor == null)
            {
                ctor = typeToConvert.GetConstructor(new[] { typeof(string), typeof(string) });
                return ctor?.Invoke(new[] { str, str }) ?? throw new ArgumentNullException("ctor");
            }
            return ctor?.Invoke(new[] { str }) ?? throw new ArgumentNullException("ctor");
        }
        return base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        destinationType == typeof(TextValue);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if(value is TextValue textValue)
        {
            return textValue.Value;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}
