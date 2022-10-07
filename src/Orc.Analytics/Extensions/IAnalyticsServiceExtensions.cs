namespace Orc.Analytics
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;

    public static class IAnalyticsServiceExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static async Task ExecuteAndTrackAsync(this IAnalyticsService analyticsService, Func<Task> func, string category,
            string variable)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            var startTime = DateTime.Now;

            await func();

            await analyticsService.QueueTimingAsync(DateTime.Now.Subtract(startTime), category, variable);
        }

        public static async Task<T> ExecuteAndTrackWithResultAsync<T>(this IAnalyticsService service, Func<Task<T>> func, string category,
            string variable)
        {
            Argument.IsNotNull("service", service);

            var startTime = DateTime.Now;

            var result = await func();

            await service.QueueTimingAsync(DateTime.Now.Subtract(startTime), category, variable);

            return result;
        }

        public static Task QueueViewModelCreatedAsync(this IAnalyticsService analyticsService, string viewModel)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            return analyticsService.QueueEventAsync("ViewModels", string.Format("{0}.Created", viewModel), viewModel);
        }

        public static async Task QueueViewModelClosedAsync(this IAnalyticsService analyticsService, string viewModel, TimeSpan duration)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            await analyticsService.QueueEventAsync("ViewModels", string.Format("{0}.Closed", viewModel), viewModel);
            await analyticsService.QueueTimingAsync(duration, "ViewModels", viewModel);
        }

        public static Task QueueCommandAsync(this IAnalyticsService analyticsService, string viewModelName, string commandName)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            var eventName = viewModelName;
            if (!string.IsNullOrEmpty(eventName))
            {
                eventName += ".";
            }
            else
            {
                eventName = string.Empty;
            }

            eventName += commandName;

            return analyticsService.QueueEventAsync("Commands", eventName);
        }

        public static async Task QueueAnalyticsValuesAsync(this IAnalyticsService analyticsService, params AnalyticsValue[] values)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            try
            {
                foreach (var analyticsValue in values)
                {
                    var valueAsString = ObjectToStringHelper.ToString(analyticsValue.Value);

                    if (analyticsValue.Value is bool)
                    {
                        valueAsString = valueAsString.ToLower();
                    }

                    await analyticsService.QueueEventAsync(analyticsValue.Category, analyticsValue.Key, valueAsString);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to submit analytics values to analytics");
            }
        }

        public static async Task QueueConfigurationValuesAsync(this IAnalyticsService analyticsService, params ConfigurationAnalyticsValue[] configurationValues)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            try
            {
#pragma warning disable IDISP001 // Dispose created
                var serviceLocator = analyticsService.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created
                var configurationService = serviceLocator.ResolveRequiredType<IConfigurationService>();

                foreach (var configurationValue in configurationValues)
                {
                    configurationValue.Value = await configurationService.GetValueAsync(configurationValue.Container, configurationValue.Key, configurationValue.DefaultValue);
                }

                await analyticsService.QueueAnalyticsValuesAsync(configurationValues);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to submit configuration values to analytics");
            }
        }
    }
}
