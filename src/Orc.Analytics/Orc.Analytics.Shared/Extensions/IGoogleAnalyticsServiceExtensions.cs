// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleAnalyticsService.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Threading.Tasks;
    using Catel;

    public static class IGoogleAnalyticsServiceExtensions
    {
        public static async Task ExecuteAndTrack(this IGoogleAnalyticsService service, Func<Task> func, string category,
            string variable)
        {
            Argument.IsNotNull("service", service);

            var startTime = DateTime.Now;

            await func();

#pragma warning disable 4014
            service.SendTiming(DateTime.Now.Subtract(startTime), category, variable);
#pragma warning restore 4014
        }

        public static async Task<T> ExecuteAndTrackWithResult<T>(this IGoogleAnalyticsService service, Func<Task<T>> func, string category,
            string variable)
        {
            Argument.IsNotNull("service", service);

            var startTime = DateTime.Now;

            var result = await func();

#pragma warning disable 4014
            service.SendTiming(DateTime.Now.Subtract(startTime), category, variable);
#pragma warning restore 4014

            return result;
        }

        public static void SendViewModelCreated(this IGoogleAnalyticsService googleAnalytics, string viewModel)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            googleAnalytics.SendEvent("ViewModels", "Created", viewModel);
        }

        public static void SendViewModelClosed(this IGoogleAnalyticsService googleAnalytics, string viewModel, TimeSpan duration)
        {
            Argument.IsNotNull("googleAnalytics", googleAnalytics);

            googleAnalytics.SendEvent("ViewModels", "Closed", viewModel);
            googleAnalytics.SendTiming(duration, "ViewModels", viewModel);
        }

        public static void SendCommand(this IGoogleAnalyticsService googleAnalytics, string viewModelName, string commandName)
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

            googleAnalytics.SendEvent("Commands", eventName);
        }
    }
}