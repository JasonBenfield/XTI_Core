using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.ComponentModel;
using XTI_Core.Extensions;

namespace XTI_Core.Tests;

sealed class TimeTest
{
    [Test]
    public void ShouldParseTime()
    {
        Assert.That
        (
            Time.Parse("10 AM"),
            Is.EqualTo(new Time(10, 0))
        );
        Assert.That
        (
            Time.Parse("10 PM"),
            Is.EqualTo(new Time(22, 0))
        );
        Assert.That
        (
            Time.Parse("14:23:22"),
            Is.EqualTo(new Time(14, 23, 22))
        );
    }

    [Test]
    public void ShouldConvertFromString()
    {
        var timeConverter = TypeDescriptor.GetConverter(typeof(Time));
        Assert.That
        (
            timeConverter.ConvertFrom("10 AM"),
            Is.EqualTo(new Time(10, 0))
        );
    }

    [Test]
    public void ShouldConvertFromJson()
    {
        var timeObj = new TimeObject { Time = new Time(15, 45) };
        var serialized = XtiSerializer.Serialize(timeObj);
        var deserialized = XtiSerializer.Deserialize<TimeObject>(serialized);
        Assert.That(deserialized.Time, Is.EqualTo(new Time(15, 45)));
    }

    private sealed class TimeObject
    {
        public Time Time { get; set; }
    }

    [Test]
    public void ShouldConvertFromOptions()
    {
        var services = setup("5:45PM");
        var options = services.GetRequiredService<TimeObject>();
        Assert.That(options?.Time, Is.EqualTo(new Time(17, 45)));
    }

    private IServiceProvider setup(string time)
    {
        var hostBuilder = new XtiHostBuilder();
        hostBuilder.Configuration.AddInMemoryCollection
        (
            new[] { KeyValuePair.Create("Time", time) }
        );
        hostBuilder.Services.AddConfigurationOptions<TimeObject>();
        return hostBuilder.Build().Scope();
    }
}