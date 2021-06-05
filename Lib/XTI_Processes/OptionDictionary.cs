using System.Collections.Generic;
using System.Linq;

namespace XTI_Processes
{
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
            if (obj != null)
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
                        options.Add(key, prop.GetValue(obj)?.ToString());
                    }
                    else if (prop.PropertyType.Equals(typeof(string)))
                    {
                        options.Add(key, (string)prop.GetValue(obj));
                    }
                    else
                    {
                        add(prop.GetValue(obj), key);
                    }
                }
            }
        }

        public Dictionary<string, string> ToDictionary()
            => options.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
