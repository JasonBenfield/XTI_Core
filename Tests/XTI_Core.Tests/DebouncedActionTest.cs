using NUnit.Framework;

namespace XTI_Core.Tests;

internal class DebouncedActionTest
{
    [Test]
    public async Task ShouldExecuteAfterTimeSpan()
    {
        var incrementer = new Incrementer();
        var originalValue = incrementer.Value;
        var timeSpan = TimeSpan.FromSeconds(1);
        var debouncedAction = new DebouncedAction
        (
            timeSpan,
            (ct) =>
            {
                incrementer.Increment();
                return Task.CompletedTask;
            }
        );
        debouncedAction.Execute(default);
        Assert.That(incrementer.Value, Is.EqualTo(originalValue));
        await Task.Delay(timeSpan.Add(TimeSpan.FromMilliseconds(100)));
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1));
    }

    [Test]
    public async Task ShouldExecuteOnceAfterTimeSpan_WhenCalledMultipleTimes()
    {
        var incrementer = new Incrementer();
        var originalValue = incrementer.Value;
        var timeSpan = TimeSpan.FromSeconds(1);
        var debouncedAction = new DebouncedAction
        (
            timeSpan,
            (ct) =>
            {
                incrementer.Increment();
                return Task.CompletedTask;
            }
        );
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        Assert.That(incrementer.Value, Is.EqualTo(originalValue));
        await Task.Delay(timeSpan.Add(TimeSpan.FromMilliseconds(100)));
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1));
    }

    [Test]
    public async Task ShouldExecuteAgainAfterFirstExecution()
    {
        var incrementer = new Incrementer();
        var originalValue = incrementer.Value;
        var timeSpan = TimeSpan.FromSeconds(1);
        var debouncedAction = new DebouncedAction
        (
            timeSpan,
            (ct) =>
            {
                incrementer.Increment();
                return Task.CompletedTask;
            }
        );
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        Assert.That(incrementer.Value, Is.EqualTo(originalValue));
        await Task.Delay(timeSpan.Add(TimeSpan.FromMilliseconds(100)));
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1));
        originalValue = incrementer.Value;
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        debouncedAction.Execute(default);
        Assert.That(incrementer.Value, Is.EqualTo(originalValue));
        await Task.Delay(timeSpan.Add(TimeSpan.FromMilliseconds(100)));
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1));
    }

    [Test]
    public async Task ShouldExecuteAfterMaxWaitTime()
    {
        var incrementer = new Incrementer();
        var originalValue = incrementer.Value;
        var timeSpan = TimeSpan.FromSeconds(1);
        var maxWaitTime = TimeSpan.FromSeconds(5);
        var debouncedAction = new DebouncedAction
        (
            timeSpan,
            (ct) =>
            {
                incrementer.Increment();
                return Task.CompletedTask;
            }
        );
        var startTime = DateTimeOffset.UtcNow;
        while(DateTimeOffset.UtcNow < startTime + maxWaitTime.Subtract(TimeSpan.FromMilliseconds(1)))
        {
            debouncedAction.Execute(default);
        }
        Assert.That(incrementer.Value, Is.EqualTo(originalValue));
        await Task.Delay(timeSpan.Add(TimeSpan.FromMilliseconds(100)));
        debouncedAction.Execute(default);
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1));
    }

    private sealed class Incrementer
    {
        public int Value { get; private set; } = 1;

        public void Increment() => Value++;
    }
}
