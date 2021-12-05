using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.ComponentModel;
using XTI_Configuration.Extensions;

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
        var options = services.GetRequiredService<IOptions<TimeObject>>().Value;
        Assert.That(options?.Time, Is.EqualTo(new Time(17, 45)));
    }

    private IServiceProvider setup(string time)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration
            (
                (hostContext, configuration) =>
                {
                    configuration.UseXtiConfiguration(hostContext.HostingEnvironment, new string[] { });
                    configuration.Sources.Clear();
                    configuration.AddInMemoryCollection(new[]
                    {
                            KeyValuePair.Create("Time", time)
                    });
                }
            )
            .ConfigureServices
            (
                (hostContext, services) =>
                {
                    services.Configure<TimeObject>(hostContext.Configuration);
                }
            )
            .Build();
        var scope = host.Services.CreateScope();
        return scope.ServiceProvider;
    }
}