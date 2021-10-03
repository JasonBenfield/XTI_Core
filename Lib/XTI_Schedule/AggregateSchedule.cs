using System;
using System.Linq;
using System.Text.Json;
using XTI_Core;

namespace XTI_Schedule
{
    public sealed class AggregateSchedule
    {
        private readonly Schedule[] schedules;

        public static AggregateSchedule Deserialize(string serialized)
           => new AggregateSchedule
            (
               JsonSerializer.Deserialize<ScheduleOptions>(serialized, getJsonSerializerOptions())
                .ToSchedules()
            );

        public AggregateSchedule(params Schedule[] schedules)
        {
            this.schedules = schedules ?? new Schedule[] { };
        }

        public bool IsInSchedule(DateTimeOffset dateTime)
        {
            dateTime = dateTime.ToLocalTime();
            return schedules.Any(s => s.IsInSchedule(dateTime));
        }

        public DateTimeRange[] DateTimeRanges(DateRange dateRange)
            => schedules
                .SelectMany(s => s.DateTimeRanges(dateRange))
                .OrderBy(dr => dr)
                .Distinct()
                .ToArray();

        public string Serialize()
            => JsonSerializer.Serialize(ToScheduleOptions(), getJsonSerializerOptions());

        private static JsonSerializerOptions getJsonSerializerOptions()
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new TimeSpanJsonConverter());
            jsonOptions.Converters.Add(new TimeJsonConverter());
            jsonOptions.Converters.Add(new MonthDayJsonConverter());
            jsonOptions.Converters.Add(new YearDayJsonConverter());
            return jsonOptions;
        }

        public ScheduleOptions ToScheduleOptions()
        {
            var aggregateOptions = schedules.Select(s => s.ToScheduleOptions());
            return new ScheduleOptions
            {
                WeeklySchedules = aggregateOptions.SelectMany(s => s.WeeklySchedules).ToArray(),
                MonthlySchedules = aggregateOptions.SelectMany(s => s.MonthlySchedules).ToArray(),
                MonthlyOrdinalSchedules = aggregateOptions.SelectMany(s => s.MonthlyOrdinalSchedules).ToArray(),
                YearlySchedules = aggregateOptions.SelectMany(s => s.YearlySchedules).ToArray(),
                YearlyOrdinalSchedules = aggregateOptions.SelectMany(s => s.YearlyOrdinalSchedules).ToArray(),
                PeriodicSchedules = aggregateOptions.SelectMany(s => s.PeriodicSchedules).ToArray()
            };
        }
    }
}
