using System.ComponentModel;
using System.Text.Json.Serialization;

namespace XTI_Core;

[TypeConverter(typeof(NumericValueTypeConverter<AppEventSeverity>))]
[JsonConverter(typeof(NumericValueJsonConverter<AppEventSeverity>))]
public sealed class AppEventSeverity : NumericValue, IEquatable<AppEventSeverity>
{
    public sealed class AppEventSeverities : NumericValues<AppEventSeverity>
    {
        public AppEventSeverities()
            : base(new AppEventSeverity(0, "Not Set"))
        {
            NotSet = DefaultValue;
            CriticalError = Add(100, "Critical Error");
            AccessDenied = Add(80, "Access Denied");
            AppError = Add(70, "App Error");
            ValidationFailed = Add(60, "Validation Failed");
            Information = Add(50, "Information");
        }

        private AppEventSeverity Add(int value, string displayText) =>
            Add(new AppEventSeverity(value, displayText));

        public AppEventSeverity NotSet { get; }
        public AppEventSeverity CriticalError { get; }
        public AppEventSeverity AccessDenied { get; }
        public AppEventSeverity ValidationFailed { get; }
        public AppEventSeverity AppError { get; }
        public AppEventSeverity Information { get; }
    }

    public static readonly AppEventSeverities Values = new();

    private AppEventSeverity(int value, string displayText)
        : base(value, displayText)
    {
    }

    public bool Equals(AppEventSeverity? other) => _Equals(other);
}