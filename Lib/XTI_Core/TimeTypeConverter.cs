using System;
using System.ComponentModel;
using System.Globalization;

namespace XTI_Core
{
    public sealed class TimeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var str = value as string;
            return str != null
                ? Time.Parse(str)
                : base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Time);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var time = (value as Time?) ?? new Time();
            return destinationType == typeof(string)
                ? time.ToString()
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
