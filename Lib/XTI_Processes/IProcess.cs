namespace XTI_Processes;

public interface IProcess
{
    Task<WinProcessResult> Run();
    string CommandText();
}