namespace XTI_Processes;

internal sealed class OptionDictionary
{
    private readonly Dictionary<string, string> options = new Dictionary<string, string>();

    public void Add(object source, string name)
    {
        var obj = source;
        add(obj, name ?? "");
    }

    private void add(object obj, string prefix)
    {
        foreach (var prop in obj.GetType().GetProperties())
        {
            string key;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                key = prop.Name;
            }
            else
            {
                key = $"{prefix}:{prop.Name}";
            }
            if (prop.PropertyType.IsValueType)
            {
                options.Add(key, prop?.GetValue(obj)?.ToString() ?? "");
            }
            else if (prop.PropertyType.Equals(typeof(string)))
            {
                var strVal = (string?)prop?.GetValue(obj);
                options.Add(key, strVal ?? "");
            }
            else
            {
                var propVal = prop.GetValue(obj);
                if (propVal != null)
                {
                    add(propVal, key);
                }
            }
        }
    }

    public Dictionary<string, string> ToDictionary()
        => options.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
}