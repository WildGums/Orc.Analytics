namespace Orc.Analytics
{
    using System;
    using System.Threading.Tasks;

    public interface IAnalyticsService
    {
        bool IsEnabled { get; set; }

        Task QueueViewAsync(string viewName);
        Task QueueEventAsync(string category, string action, string? label = null, long value = 0);
        Task QueueTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1);
        Task QueueTimingAsync(TimeSpan time, string category, string variable, string? label = null);

        Task SendAsync();
    }
}
