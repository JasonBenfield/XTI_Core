using System.Text.Json;

namespace XTI_Core.Tests;

internal static class TestExtensions
{
    public static void WriteToConsole(this object data) =>
        Console.WriteLine
        (
            XtiSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true })
        );
}