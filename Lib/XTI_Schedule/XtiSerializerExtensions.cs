using System.Text.Json;
using XTI_Core;

namespace XTI_Schedule;

public static class XtiSerializerExtensions
{
    public static JsonSerializerOptions AddScheduleConverters(this JsonSerializerOptions options)
    {
        options.AddCoreConverters();
        if (!options.Converters.OfType<MonthDayJsonConverter>().Any())
        {
            options.Converters.Add(new MonthDayJsonConverter());
        }
        if (!options.Converters.OfType<YearDayJsonConverter>().Any())
        {
            options.Converters.Add(new YearDayJsonConverter());
        }
        return options;
    }
}