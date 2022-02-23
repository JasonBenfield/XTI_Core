namespace XTI_Processes;

public sealed class DataReceivedEventArgs : EventArgs
{
    public DataReceivedEventArgs(string data)
    {
        Data = data;
    }

    public string Data { get; }
}
