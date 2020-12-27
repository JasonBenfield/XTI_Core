using System;

namespace XTI_Core
{
    public interface Clock
    {
        DateTimeOffset Now();
        DateTimeOffset Today();
    }
}
