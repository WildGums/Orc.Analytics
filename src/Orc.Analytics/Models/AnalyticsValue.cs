namespace Orc.Analytics
{
    public class AnalyticsValue
    {
        public AnalyticsValue()
        {
            Category = "General";
            Key = string.Empty;
        }

        public string Category { get; set; }

        public string Key { get; set; }

        public object? Value { get; set; }
    }
}
