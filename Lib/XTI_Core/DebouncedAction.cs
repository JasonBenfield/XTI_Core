namespace XTI_Core;

public sealed class DebouncedAction
{
    private readonly TimeSpan interval;
    private readonly Func<CancellationToken, Task> action;
    private readonly TimeSpan maxWaitTime;

    private DateTimeOffset timeToForceExecution = DateTimeOffset.MaxValue;
    private Timer? timer;

    public DebouncedAction(TimeSpan interval, Func<CancellationToken, Task> action)
        : this(interval, action, TimeSpan.FromHours(1))
    {
    }

    public DebouncedAction(TimeSpan interval, Func<CancellationToken, Task> action, TimeSpan maxWaitTime)
    {
        this.interval = interval;
        this.action = action;
        this.maxWaitTime = maxWaitTime;
    }

    public void Execute(CancellationToken ct)
    {
        timer?.Dispose();
        timer = null;
        if (timeToForceExecution == DateTimeOffset.MaxValue)
        {
            timeToForceExecution = DateTimeOffset.UtcNow + maxWaitTime;
        }
        if (DateTimeOffset.UtcNow > timeToForceExecution)
        {
            timeToForceExecution = DateTimeOffset.MaxValue;
            var t = action(ct);
        }
        else
        {
            timer = new Timer
            (
                async (state) => await onTimeElapsed(state, ct),
                null,
                interval,
                Timeout.InfiniteTimeSpan
            );
        }
    }

    private async Task onTimeElapsed(object? _, CancellationToken ct)
    {
        timeToForceExecution = DateTimeOffset.MaxValue;
        if (timer != null)
        {
            timer?.Dispose();
            timer = null;
            await action(ct);
        }
    }
}
