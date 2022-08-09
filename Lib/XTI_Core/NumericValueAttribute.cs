namespace XTI_Core;

public sealed class NumericValueAttribute : Attribute
{
    public NumericValueAttribute(Type dataType)
    {
        DataType = dataType;
    }

    public Type DataType { get; }

    public override string ToString() => $"{nameof(NumericValueAttribute)} {DataType.Name}";
}
