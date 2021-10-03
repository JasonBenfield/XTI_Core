using System;
using System.ComponentModel;
using System.Globalization;

namespace XTI_Schedule
{
    public sealed class YearDayTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            return str != null
                ? YearDay.Parse(str)
                : base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(YearDay);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var yearDay = (value as YearDay?) ?? new YearDay();
            return destinationType == typeof(string)
                ? yearDay.Format()
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
