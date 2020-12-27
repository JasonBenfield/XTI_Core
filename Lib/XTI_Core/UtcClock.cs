using System;

namespace XTI_Core
{
    public sealed class UtcClock : Clock
    {
        public DateTimeOffset Now() => DateTimeOffset.UtcNow;

        public DateTimeOffset Today() => DateTimeOffset.UtcNow.Date;
    }
}
