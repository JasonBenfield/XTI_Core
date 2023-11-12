using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace XTI_Core.Tests;

internal class SingleThreadedActionTest
{
    [Test]
    public async Task ShouldOnlyAllowOneExecutionAtATime()
    {
        var sp = new ServiceCollection().BuildServiceProvider();
        var incrementer = new Incrementer();
        var originalValue = incrementer.Value;
        var singleThreadAction = new SingleThreadedAction
        (
            async (IServiceProvider _, CancellationToken _) =>
            {
                Console.WriteLine($"{DateTimeOffset.Now:HH:mm:ss.fff} Incrementing from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
                incrementer.Increment();
                Console.WriteLine($"{DateTimeOffset.Now:HH:mm:ss.fff} Incremented from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"{DateTimeOffset.Now:HH:mm:ss.fff} After Delay from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
            }
        );
        var t = Task.Run
        (
            () =>
            {
                var t1 = singleThreadAction.Execute(sp);
                var t2 = singleThreadAction.Execute(sp);
                var t3 = singleThreadAction.Execute(sp);
            }
        );
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} Delaying from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} After Delay from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 1), "Should increment from first thread");
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} Delaying from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        await Task.Delay(TimeSpan.FromSeconds(1).Add(TimeSpan.FromMilliseconds(100)));
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} After Delay from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 2), "Should increment from second thread");
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} Delaying from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        await Task.Delay(TimeSpan.FromSeconds(1).Add(TimeSpan.FromMilliseconds(100)));
        Console.WriteLine($"TEST BODY {DateTimeOffset.Now:HH:mm:ss.fff} After Delay from Thread {Thread.CurrentThread.ManagedThreadId}: CurrentValue {incrementer.Value}");
        Assert.That(incrementer.Value, Is.EqualTo(originalValue + 3), "Should increment from third thread");
    }

    private sealed class Incrementer
    {
        public int Value { get; private set; } = 1;

        public void Increment() => Value++;
    }
}
