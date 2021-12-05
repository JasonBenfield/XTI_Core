namespace XTI_Core;

public sealed class UtcClock : IClock
{
    public DateTimeOffset Now() => DateTimeOffset.UtcNow;

    public DateTimeOffset Today() => DateTimeOffset.UtcNow.Date;
}