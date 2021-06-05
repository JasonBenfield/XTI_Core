namespace XTI_Processes
{
    public sealed record WinProcessResult(int ExitCode, string[] OutputLines, string[] ErrorLines);
}
