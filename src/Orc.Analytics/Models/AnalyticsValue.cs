namespace Orc.Analytics
{
    public class AnalyticsValue
    {
        public AnalyticsValue()
        {
            Category = "General";
        }

        public string Category { get; protected set; }

        public string Key { get; set; }

        public object Value { get; set; }
    }
}
