// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleAnalyticsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IGoogleAnalyticsService
    {
        #region Properties
        bool IsEnabled { get; set; }
        #endregion

        #region Methods
        Task SendView(string viewName);
        Task SendEvent(string category, string action, string label = null, long value = 0);
        Task SendTransaction(string sku, string name, string transactionId, long costPerProduct, int quantity = 1);
        Task SendTiming(TimeSpan time, string category, string variable, string label = "");
        #endregion
    }
}