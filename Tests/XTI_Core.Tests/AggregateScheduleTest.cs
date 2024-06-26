﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XTI_Core.Extensions;
using XTI_Schedule;

namespace XTI_Core.Tests;

internal sealed class AggregateScheduleTest
{
    [Test]
    public void ShouldBeInScheduleByDayOfWeek()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.On(DayOfWeek.Monday).At(TimeRange.AllDay())
        );
        var result = schedule.IsInSchedule(new DateTime(2020, 10, 19, 12, 0, 0));
        Assert.That(result, Is.True, "Should be scheduled by day of week");
    }

    [Test]
    public void ShouldNotBeInSchedule_WhenNotTheDayOfWeek()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.On(DayOfWeek.Monday).At(TimeRange.AllDay())
        );
        var result = schedule.IsInSchedule(new DateTime(2020, 10, 20, 12, 0, 0));
        Assert.That(result, Is.False, "Should not be scheduled when not the day of week");
    }

    [Test]
    public void ShouldNotBeInSchedule_WhenNotInTimeRange()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.On(DayOfWeek.Monday)
                .At(TimeRange.From(10, 0).For(2).Hours())
        );
        var result = schedule.IsInSchedule(new DateTime(2020, 10, 19, 8, 0, 0));
        Assert.That(result, Is.False, "Should not be in schedule");
    }

    [Test]
    public void ShouldBeInSchedule_WhenDateTimeIsInAnyOfTheOptionTimeRange()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.On(DayOfWeek.Monday)
                .At
                (
                    TimeRange.From(10, 0).For(2).Hours(),
                    TimeRange.From(17, 0).For(2).Hours()
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
            Schedule.On(DayOfWeek.Monday, DayOfWeek.Wednesday)
                .At
                (
                    TimeRange.From(10, 0).For(2).Hours(),
                    TimeRange.From(17, 0).For(2).Hours()
                )
        );
        var result = schedule.IsInSchedule(new DateTime(2020, 10, 21, 18, 0, 0));
        Assert.That(result, Is.True, "Should be in schedule");
    }

    [Test]
    public void ShouldGetTimeRangesWithMultipleDayRangesAndTimeRanges()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.EveryDay().At(TimeRange.From(new TimeOnly(11, 30)).ForOneHour()),
            Schedule.On(DayOfWeek.Thursday).At
            (
                TimeRange.From(new TimeOnly(8, 0)).ForOneHour(),
                TimeRange.From(new TimeOnly(14, 0)).ForOneHour()
            )
        );
        var dateTimeRanges = schedule.DateTimeRanges(DateRange.From(new DateOnly(2023, 2, 1)).Until(new DateOnly(2023, 2, 2)));
        Assert.That
        (
            dateTimeRanges,
            Is.EquivalentTo
            (
                new[]
                {
                    DateTimeRange.From(new DateTime(2023, 2, 1, 11, 30, 0)).Until(new DateTime(2023, 2, 1, 12, 30, 0)),
                    DateTimeRange.From(new DateTime(2023, 2, 2, 8, 0, 0)).Until(new DateTime(2023, 2, 2, 9, 0, 0)),
                    DateTimeRange.From(new DateTime(2023, 2, 2, 11, 30, 0)).Until(new DateTime(2023, 2, 2, 12, 30, 0)),
                    DateTimeRange.From(new DateTime(2023, 2, 2, 14, 0, 0)).Until(new DateTime(2023, 2, 2, 15, 0, 0))
                }
            )
        );
    }

    [Test]
    public void ShouldDeserializeSchedule()
    {
        var schedule = new AggregateSchedule
        (
            Schedule.On(DayOfWeek.Monday)
                .At(TimeRange.From(10, 0).ForOneHour()),
            Schedule.On(new MonthDay(15))
                .At(TimeRange.From(11, 0).ForOneHour()),
            Schedule.First(DayOfWeek.Monday).OfEveryMonth()
                .At(TimeRange.From(12, 0).ForOneHour()),
            Schedule.On(new YearDay(10, 13))
                .At(TimeRange.From(13, 0).ForOneHour()),
            Schedule.First(DayOfWeek.Tuesday).Of(Months.October)
                .At(TimeRange.From(14, 0).ForOneHour()),
            Schedule.Every(2).Weeks().Starting(new DateTime(2021, 10, 12))
                .At(TimeRange.From(15, 0).ForOneHour())
        );
        var serialized = schedule.Serialize();
        var deserialized = AggregateSchedule.Deserialize(serialized);
        Assert.That
        (
            deserialized.DateTimeRanges
            (
                DateRange.Between(new DateTime(2021, 10, 1), new DateTime(2021, 10, 31))
            ),
            Is.EqualTo
            (
                new[]
                {
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 4, 10, 0, 0),
                            new DateTime(2021, 10, 4, 11, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 4, 12, 0, 0),
                            new DateTime(2021, 10, 4, 13, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 5, 14, 0, 0),
                            new DateTime(2021, 10, 5, 15, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 11, 10, 0, 0),
                            new DateTime(2021, 10, 11, 11, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 12, 15, 0, 0),
                            new DateTime(2021, 10, 12, 16, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 13, 13, 0, 0),
                            new DateTime(2021, 10, 13, 14, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 15, 11, 0, 0),
                            new DateTime(2021, 10, 15, 12, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 18, 10, 0, 0),
                            new DateTime(2021, 10, 18, 11, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 25, 10, 0, 0),
                            new DateTime(2021, 10, 25, 11, 0, 0)
                        ),
                        DateTimeRange.Between
                        (
                            new DateTime(2021, 10, 26, 15, 0, 0),
                            new DateTime(2021, 10, 26, 16, 0, 0)
                        )
                }
            )
        );
    }

    [Test]
    public void ShouldParseAppSettingsForWeeklySchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("WeeklySchedules:0:Days:0", "Monday"),
                KeyValuePair.Create("WeeklySchedules:0:Days:1", "Wednesday"),
                KeyValuePair.Create("WeeklySchedules:0:TimeRanges:0:Start", "10 AM"),
                KeyValuePair.Create("WeeklySchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
        Assert.That(options.WeeklySchedules.Length, Is.EqualTo(1));
        Assert.That
        (
            options.WeeklySchedules[0].Days,
            Is.EqualTo(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday })
        );
        Assert.That(options.WeeklySchedules[0].TimeRanges.Length, Is.EqualTo(1));
        Assert.That(options.WeeklySchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.WeeklySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    [Test]
    public void ShouldParseAppSettingsForMonthlySchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("MonthlySchedules:0:Days:0", "15"),
                KeyValuePair.Create("MonthlySchedules:0:Days:1", "Last"),
                KeyValuePair.Create("MonthlySchedules:0:TimeRanges:0:Start", "10 AM"),
                KeyValuePair.Create("MonthlySchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
        Assert.That
        (
            options.MonthlySchedules[0].Days,
            Is.EqualTo(new[] { new MonthDay(15), MonthDay.LastDay })
        );
        Assert.That(options.MonthlySchedules[0].TimeRanges.Length, Is.EqualTo(1));
        Assert.That(options.MonthlySchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.MonthlySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    [Test]
    public void ShouldParseAppSettingsForMonthlyOrdinalSchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("MonthlyOrdinalSchedules:0:Days:0:Week", "Second"),
                KeyValuePair.Create("MonthlyOrdinalSchedules:0:Days:0:DayOfWeek", "Thursday"),
                KeyValuePair.Create("MonthlyOrdinalSchedules:0:TimeRanges:0:Start", "10 AM"),
                KeyValuePair.Create("MonthlyOrdinalSchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
        Assert.That(options.MonthlyOrdinalSchedules.Length, Is.EqualTo(1));
        Assert.That(options.MonthlyOrdinalSchedules[0].Days.Length, Is.EqualTo(1));
        Assert.That(options.MonthlyOrdinalSchedules[0].Days[0].Week, Is.EqualTo(OrdinalWeek.Second));
        Assert.That(options.MonthlyOrdinalSchedules[0].Days[0].DayOfWeek, Is.EqualTo(DayOfWeek.Thursday));
        Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges.Length, Is.EqualTo(1));
        Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.MonthlyOrdinalSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    [Test]
    public void ShouldParseAppSettingsForPeriodicSchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("PeriodicSchedules:0:Frequency", "2"),
                KeyValuePair.Create("PeriodicSchedules:0:Interval", "Weeks"),
                KeyValuePair.Create("PeriodicSchedules:0:StartDate", "10/2/2021"),
                KeyValuePair.Create("PeriodicSchedules:0:TimeRanges:0:Start", "10 AM"),
                KeyValuePair.Create("PeriodicSchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
        Assert.That(options.PeriodicSchedules.Length, Is.EqualTo(1));
        Assert.That(options.PeriodicSchedules[0].Frequency, Is.EqualTo(2));
        Assert.That(options.PeriodicSchedules[0].Interval, Is.EqualTo(DateInterval.Weeks));
        Assert.That(options.PeriodicSchedules[0].StartDate, Is.EqualTo(new DateOnly(2021, 10, 2)));
        Assert.That(options.PeriodicSchedules[0].TimeRanges.Length, Is.EqualTo(1));
        Assert.That(options.PeriodicSchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.PeriodicSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    [Test]
    public void ShouldParseAppSettingsForYearlySchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("YearlySchedules:0:Days:0", "4/18"),
                KeyValuePair.Create("YearlySchedules:0:Days:1", "9/7"),
                KeyValuePair.Create("YearlySchedules:0:TimeRanges:0:Start", "10:00"),
                KeyValuePair.Create("YearlySchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
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
        Assert.That(options.YearlySchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.YearlySchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    [Test]
    public void ShouldParseAppSettingsForYearlyOrdinalSchedules()
    {
        var services = Setup
        (
            [
                KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Week", "Second"),
                KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:DayOfWeek", "Thursday"),
                KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Months:0", "March"),
                KeyValuePair.Create("YearlyOrdinalSchedules:0:Days:0:Months:1", "May"),
                KeyValuePair.Create("YearlyOrdinalSchedules:0:TimeRanges:0:Start", "10 AM"),
                KeyValuePair.Create("YearlyOrdinalSchedules:0:TimeRanges:0:Duration", "2:30:00")
            ]
        );
        var options = services.GetRequiredService<ScheduleOptions>();
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
        Assert.That(options.YearlyOrdinalSchedules[0].TimeRanges[0].Start, Is.EqualTo(new TimeOnly(10, 0)));
        Assert.That(options.YearlyOrdinalSchedules[0].TimeRanges[0].Duration, Is.EqualTo(new TimeSpan(2, 30, 0)));
    }

    private IServiceProvider Setup(KeyValuePair<string, string>[] settings)
    {
        var builder = new XtiHostBuilder();
        builder.Configuration.AddInMemoryCollection
        (
            settings.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value))
        );
        builder.Services.AddConfigurationOptions<ScheduleOptions>();
        return builder.Build().Scope();
    }
}