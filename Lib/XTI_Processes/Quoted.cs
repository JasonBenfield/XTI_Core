namespace XTI_Processes
{
    public sealed class Quoted
    {
        private readonly string value;

        public Quoted(string value)
        {
            this.value = $"\"{value}\"";
        }

        public string Value() => value;

        public override string ToString() => $"{nameof(Quoted)} {value}";
    }
}
