using System;
using System.Text.Json;

namespace XTI_Tool
{
    public sealed class XtiProcessData
    {
        public void Output(object obj)
        {
            if (obj != null)
            {
                var serialized = JsonSerializer.Serialize(obj);
                Console.WriteLine($"<data>{serialized}</data>");
            }
        }
    }
}
