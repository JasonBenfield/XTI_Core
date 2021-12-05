namespace XTI_Core.Fakes;

public sealed class FakeClock : IClock
{
    public FakeClock(DateTimeOffset? now = null)
    {
        this.now = now ?? DateTimeOffset.Now;
    }

    private DateTimeOffset now;

    public DateTimeOffset Now() => now;

    public DateTimeOffset Today() => now.Date;

    public void Set(DateTimeOffset now) => this.now = now;

    public void Add(TimeSpan timeSpan) => now = now.Add(timeSpan);

    public override string ToString() => $"{nameof(FakeClock)} {now:M/dd/yy h:mm:ss tt}";
}