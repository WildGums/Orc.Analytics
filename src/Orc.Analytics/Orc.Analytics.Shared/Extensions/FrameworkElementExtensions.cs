// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.Analytics
{
    using System.Windows;
    using Catel;
    using Catel.IoC;

    public static partial class FrameworkElementExtensions
    {
        private static readonly IGoogleAnalyticsService AnalyticsService = ServiceLocator.Default.ResolveType<IGoogleAnalyticsService>();

        public static void TrackViewForAnalytics(this FrameworkElement frameworkElement)
        {
            Argument.IsNotNull("frameworkElement", frameworkElement);

            AnalyticsService.SendView(frameworkElement.GetType().Name);
        }
    }
}