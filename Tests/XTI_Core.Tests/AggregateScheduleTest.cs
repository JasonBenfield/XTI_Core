using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using XTI_Configuration.Extensions;
using XTI_Schedule;

namespace XTI_Core.Tests
{
    public sealed class AggregateScheduleTest
    {
        [Test]
        public void ShouldBeInScheduleByDayOfWeek()
        {
            var schedule = new AggregateSchedule
            (
                new Schedule
                (
                    new WeeklySchedule(DayOfWeek.Monday),
                    TimeRange.AllDay()
                )
            );
            var result = schedule.IsInSchedule(new DateTime(2020, 10, 19, 12, 0, 0));
            Assert.That(result, Is.True, "Should be scheduled by day of week");
        }

        [Test]
        public void ShouldNotBeInSchedule_WhenNotTheDayOfWeek()
        {
            var schedule = new AggregateSchedule
            (
                new Schedule
                (
                    new WeeklySchedule(DayOfWeek.Monday),
                    TimeRange.AllDay()
                )
            );
            var result = schedule.IsInSchedule(new DateTime(2020, 10, 20, 12, 0, 0));
            Assert.That(result, Is.False, "Should not be scheduled when not the day of week");
        }

        [Test]
        public void ShouldNotBeInSchedule_WhenNotInTimeRange()
        {
            var schedule = new AggregateSchedule
            (
                new Schedule
                (
                    new WeeklySchedule(DayOfWeek.Monday),
                    new TimeRange(new Time(10, 0), TimeSpan.FromHours(2))
                )
            );
            var result = schedule.IsInSchedule(new DateTime(2020, 10, 19, 8, 0, 0));
            Assert.That(result, Is.False, "Should not be in schedule");
        }

        [Test]
        public void ShouldBeInSchedule_WhenDateTimeIsInAnyOfTheOptionTimeRange()
        {
            var schedule = new AggregateSchedule
            (
                new Schedule
                (
                    new WeeklySchedule(DayOfWeek.Monday),
                    new TimeRange(new Time(10, 0), TimeSpan.FromHours(2)),
                    new TimeRange(new Time(17, 0), TimeSpan.FromHours(2))
                )
            );
            var result = schedule.IsInSchedule(new DateTime(2020, 10, 19, 18, 0, 0));
            Assert.That(result, Is.True, "Should be in schedule");
        }

        [Test]
        public void ShouldBeInSchedule_WhenDateTimeIsInAnyOfTheOptionDaysOfTheWeek()
        {
            var schedule = new AggregateSchedule
            (
                new Schedule
                (
                    new WeeklySchedule(DayOfWeek.Monday, DayOfWeek.Wednesday),
                    new TimeRange(new Time(10, 0), TimeSpan.FromHours(2)),
                    new TimeRange(new Time(17, 0), TimeSpan.FromHours(2))
                )
            );
            var result = schedule.IsInSchedule(new DateTime(2020, 10, 21, 18, 0, 0));
            Assert.That(result, Is.True, "Should be in schedule");
        }

        [Test]
        public void ShouldParseAppSettingsForWeeklySchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("WeeklySchedules:0:Days:0", "Monday"),
                    KeyValuePair.Create("WeeklySchedules:0:Days:1", "Wednesday"),
                    KeyValuePair.Create("WeeklySchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("WeeklySchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That(options.WeeklySchedules.Length, Is.EqualTo(1));
            Assert.That
            (
                options.WeeklySchedules[0].Days,
                Is.EqualTo(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday })
            );
            Assert.That(options.WeeklySchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.WeeklySchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.WeeklySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        [Test]
        public void ShouldParseAppSettingsForMonthlySchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("MonthlySchedules:0:Days:0", "15"),
                    KeyValuePair.Create("MonthlySchedules:0:Days:1", "Last"),
                    KeyValuePair.Create("MonthlySchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("MonthlySchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That
            (
                options.MonthlySchedules[0].Days,
                Is.EqualTo(new[] { new MonthDay(15), MonthDay.LastDay })
            );
            Assert.That(options.MonthlySchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.MonthlySchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.MonthlySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        [Test]
        public void ShouldParseAppSettingsForMonthlyOrdinalSchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("MonthlyOrdinalSchedules:0:Days:0:Week", "Second"),
                    KeyValuePair.Create("MonthlyOrdinalSchedules:0:Days:0:DayOfWeek", "Thursday"),
                    KeyValuePair.Create("MonthlyOrdinalSchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("MonthlyOrdinalSchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That(options.MonthlyOrdinalSchedules.Length, Is.EqualTo(1));
            Assert.That(options.MonthlyOrdinalSchedules[0].Days.Length, Is.EqualTo(1));
            Assert.That(options.MonthlyOrdinalSchedules[0].Days[0].Week, Is.EqualTo(OrdinalWeek.Second));
            Assert.That(options.MonthlyOrdinalSchedules[0].Days[0].DayOfWeek, Is.EqualTo(DayOfWeek.Thursday));
            Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        [Test]
        public void ShouldParseAppSettingsForPeriodicSchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("PeriodicSchedules:0:Frequency", "2"),
                    KeyValuePair.Create("PeriodicSchedules:0:Interval", "Weeks"),
                    KeyValuePair.Create("PeriodicSchedules:0:StartDate", "10/2/2021"),
                    KeyValuePair.Create("PeriodicSchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("PeriodicSchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That(options.PeriodicSchedules.Length, Is.EqualTo(1));
            Assert.That(options.PeriodicSchedules[0].Frequency, Is.EqualTo(2));
            Assert.That(options.PeriodicSchedules[0].Interval, Is.EqualTo(DateInterval.Weeks));
            Assert.That(options.PeriodicSchedules[0].StartDate, Is.EqualTo(new DateTime(2021, 10, 2)));
            Assert.That(options.PeriodicSchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.PeriodicSchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.PeriodicSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        [Test]
        public void ShouldParseAppSettingsForYearlySchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("YearlySchedules:0:Days:0", "4/18"),
                    KeyValuePair.Create("YearlySchedules:0:Days:1", "9/7"),
                    KeyValuePair.Create("YearlySchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("YearlySchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That(options.YearlySchedules.Length, Is.EqualTo(1));
            Assert.That
            (
                options.YearlySchedules[0].Days,
                Is.EqualTo
                (
                    new[] { new YearDay(4, 18), new YearDay(9, 7) }
                )
            );
            Assert.That(options.YearlySchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.YearlySchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.YearlySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        [Test]
        public void ShouldParseAppSettingsForYearlyOrdinalSchedules()
        {
            var services = setup
            (
                new[]
                {
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Week", "Second"),
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:DayOfWeek", "Thursday"),
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Months:0", "March"),
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Months:1", "May"),
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:TimeRanges:0:Start", "10 AM"),
                    KeyValuePair.Create("YearlyOrdinalSchedules:0:TimeRanges:0:Duration", "2:30:00")
                }
            );
            var options = services.GetService<IOptions<ScheduleOptions>>().Value;
            Assert.That(options.YearlyOrdinalSchedules.Length, Is.EqualTo(1));
            Assert.That(options.YearlyOrdinalSchedules[0].Days.Length, Is.EqualTo(1));
            Assert.That(options.YearlyOrdinalSchedules[0].Days[0].Week, Is.EqualTo(OrdinalWeek.Second));
            Assert.That(options.YearlyOrdinalSchedules[0].Days[0].DayOfWeek, Is.EqualTo(DayOfWeek.Thursday));
            Assert.That
            (
                options.YearlyOrdinalSchedules[0].Days[0].Months,
                Is.EqualTo(new[] { Months.March, Months.May })
            );
            Assert.That(options.YearlyOrdinalSchedules[0].TimeRanges.Length, Is.EqualTo(1));
            Assert.That(options.YearlyOrdinalSchedules[0].TimeRanges[0].Start, Is.EqualTo(new Time(10, 0)));
            Assert.That(options.YearlyOrdinalSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
        }

        private IServiceProvider setup(KeyValuePair<string, string>[] settings)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration
                (
                    (hostContext, configuration) =>
                    {
                        configuration.UseXtiConfiguration(hostContext.HostingEnvironment, new string[] { });
                        configuration.Sources.Clear();
                        configuration.AddInMemoryCollection(settings);
                    }
                )
                .ConfigureServices
                (
                    (hostContext, services) =>
                    {
                        services.Configure<ScheduleOptions>(hostContext.Configuration);
                    }
                )
                .Build();
            var scope = host.Services.CreateScope();
            return scope.ServiceProvider;
        }
    }
}
