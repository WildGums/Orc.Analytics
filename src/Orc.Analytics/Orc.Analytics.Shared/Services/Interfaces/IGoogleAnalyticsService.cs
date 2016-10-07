// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGoogleAnalyticsService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics
{
    using System;
    using System.Threading.Tasks;

    public interface IGoogleAnalyticsService
    {
        #region Properties
        bool IsEnabled { get; set; }
        string AccountId { get; set; }
        string UserId { get; set; }
        string AppName { get; set; }
        string AppVersion { get; set; }
        #endregion

        #region Methods
        Task SendViewAsync(string viewName);
        Task SendEventAsync(string category, string action, string label = null, long value = 0);
        Task SendTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1);
        Task SendTimingAsync(TimeSpan time, string category, string variable, string label = "");
        #endregion
    }
}