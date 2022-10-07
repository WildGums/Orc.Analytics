namespace Orc.Analytics
{
    using System.Windows;
    using Catel.IoC;
    using System.Threading.Tasks;
    using System;

    public static partial class FrameworkElementExtensions
    {
        private static readonly IAnalyticsService AnalyticsService = ServiceLocator.Default.ResolveRequiredType<IAnalyticsService>();

        public static Task TrackViewForAnalyticsAsync(this FrameworkElement frameworkElement)
        {
            ArgumentNullException.ThrowIfNull(frameworkElement);

            return AnalyticsService.QueueViewAsync(frameworkElement.GetType().Name);
        }
    }
}
