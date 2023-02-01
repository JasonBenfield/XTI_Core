using XTI_Core;

namespace XTI_Schedule;

public interface IDaySchedule
{
    bool IsInRange(DateOnly value);
    DateOnly[] AllowedDates(DateRange range);
}