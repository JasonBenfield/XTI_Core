using System.ComponentModel;
using System.Globalization;

namespace XTI_Schedule;

public sealed class MonthDayTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => 
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var str = value as string;
        return str != null
            ? MonthDay.Parse(str)
            : base.ConvertFrom(context, culture, value);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)=> 
        destinationType == typeof(MonthDay);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        var monthDay = (value as MonthDay?) ?? new MonthDay();
        return destinationType == typeof(string)
            ? monthDay.Format()
            : base.ConvertTo(context, culture, value, destinationType);
    }
}