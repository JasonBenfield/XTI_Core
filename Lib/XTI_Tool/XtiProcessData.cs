using System.Text.Json;

namespace XTI_Tool;

public sealed class XtiProcessData
{
    public void Output(object obj)
    {
        var serialized = JsonSerializer.Serialize(obj);
        Console.WriteLine($"<data>{serialized}</data>");
    }
}