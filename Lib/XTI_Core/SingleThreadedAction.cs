namespace XTI_Core;

public sealed class SingleThreadedAction
{
    private readonly Semaphore semaphore;
    private readonly Func<IServiceProvider, CancellationToken, Task> action;

    public SingleThreadedAction(Func<IServiceProvider, CancellationToken, Task> action)
        : this(action, Guid.NewGuid().ToString("D"))
    {
    }

    public SingleThreadedAction(Func<IServiceProvider, CancellationToken, Task> action, string processID)
    {
        semaphore = new(1, 1, processID);
        this.action = action;
    }

    public async Task Execute(IServiceProvider sp, CancellationToken ct = default)
    {
        semaphore.WaitOne();
        try
        {
            await action(sp, ct);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
