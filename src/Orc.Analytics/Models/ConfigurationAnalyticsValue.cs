namespace Orc.Analytics;

using System;
using Catel.Configuration;

public class ConfigurationAnalyticsValue : AnalyticsValue
{
    public ConfigurationAnalyticsValue(ConfigurationContainer container, string key, object? defaultValue)
    {
        ArgumentNullException.ThrowIfNull(container);

        Category = "Configuration";
        Container = container;
        Key = key;
        DefaultValue = defaultValue;
    }

    public ConfigurationContainer Container { get; set; }

    public object? DefaultValue { get; set; }
}
