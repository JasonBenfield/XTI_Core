using System;
using XTI_Core;

namespace XTI_Schedule
{
    public interface IDaySchedule
    {
        bool IsInRange(DateTimeOffset value);
        DateTime[] AllowedDates(DateRange range);
    }
}
