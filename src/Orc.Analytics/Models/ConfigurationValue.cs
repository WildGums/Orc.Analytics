namespace Orc.Analytics
{
    using Catel.Configuration;

    public class ConfigurationValue : AnalyticsValue
    {
        public ConfigurationValue(ConfigurationContainer container, string key, object defaultValue)
        {
            Category = "Configuration";
            Container = container;
            Key = key;
            DefaultValue = defaultValue;
        }

        public ConfigurationContainer Container { get; set; }

        public object DefaultValue { get; set; }
    }
}
