namespace XTI_Core;

public interface IClock
{
    DateTimeOffset Now();
    DateTimeOffset Today();
}