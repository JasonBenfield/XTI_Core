using System;

namespace XTI_Processes.TestAppConfig
{
    public sealed class NamedOptions
    {
        public static readonly string Named = nameof(Named);
        public string Value1 { get; set; }
        public NestedOptions Nested { get; set; }
    }

    public sealed class NestedOptions
    {
        public DateTimeOffset SomeTime { get; set; }
    }
}
