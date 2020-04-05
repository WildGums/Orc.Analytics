// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAnalyticsService.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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

        public static async Task ExecuteAndTrackAsync(this IAnalyticsService service, Func<Task> func, string category,
            string variable)
        {
            Argument.IsNotNull("service", service);

            var startTime = DateTime.Now;

            await func();

#pragma warning disable 4014
            service.SendTimingAsync(DateTime.Now.Subtract(startTime), category, variable);
#pragma warning restore 4014
        }

        public static async Task<T> ExecuteAndTrackWithResultAsync<T>(this IAnalyticsService service, Func<Task<T>> func, string category,
            string variable)
        {
            Argument.IsNotNull("service", service);

            var startTime = DateTime.Now;

            var result = await func();

#pragma warning disable 4014
            service.SendTimingAsync(DateTime.Now.Subtract(startTime), category, variable);
#pragma warning restore 4014

            return result;
        }

        public static Task SendViewModelCreatedAsync(this IAnalyticsService googleAnalytics, string viewModel)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            return googleAnalytics.SendEventAsync("ViewModels", string.Format("{0}.Created", viewModel), viewModel);
        }

        public static async Task SendViewModelClosedAsync(this IAnalyticsService googleAnalytics, string viewModel, TimeSpan duration)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            await googleAnalytics.SendEventAsync("ViewModels", string.Format("{0}.Closed", viewModel), viewModel);
            await googleAnalytics.SendTimingAsync(duration, "ViewModels", viewModel);
        }

        public static Task SendCommandAsync(this IAnalyticsService googleAnalytics, string viewModelName, string commandName)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

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

            return googleAnalytics.SendEventAsync("Commands", eventName);
        }

        public static async Task SendAnalyticsValuesAsync(this IAnalyticsService googleAnalytics, params AnalyticsValue[] values)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            try
            {
                foreach (var analyticsValue in values)
                {
                    var valueAsString = ObjectToStringHelper.ToString(analyticsValue.Value);

                    if (analyticsValue.Value is bool)
                    {
                        valueAsString = valueAsString.ToLower();
                    }

                    await googleAnalytics.SendEventAsync(analyticsValue.Category, analyticsValue.Key, valueAsString);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to submit analytics values to analytics");
            }
        }

            public static async Task SendConfigurationValuesAsync(this IAnalyticsService googleAnalytics, params ConfigurationAnalyticsValue[] configurationValues)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            try
            {
                var serviceLocator = googleAnalytics.GetServiceLocator();
                var configurationService = serviceLocator.ResolveType<IConfigurationService>();

                foreach (var configurationValue in configurationValues)
                {
                    configurationValue.Value = configurationService.GetValue(configurationValue.Container, configurationValue.Key, configurationValue.DefaultValue);
                }

                await googleAnalytics.SendAnalyticsValuesAsync(configurationValues);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to submit configuration values to analytics");
            }
        }
    }
}
