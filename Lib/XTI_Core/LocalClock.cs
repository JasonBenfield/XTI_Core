namespace XTI_Core;

public sealed class LocalClock : IClock
{
    public DateTimeOffset Now() => DateTimeOffset.Now;

    public DateTimeOffset Today() => DateTimeOffset.Now.Date;
}