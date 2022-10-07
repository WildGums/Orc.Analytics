namespace Orc.Analytics
{
    using System;
    using System.Threading.Tasks;
    using Catel.Logging;

    public class EmptyAnalyticsService : IAnalyticsService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public EmptyAnalyticsService()
        {
            IsEnabled = false;
        }

        public bool IsEnabled { get; set; }

        public virtual Task QueueViewAsync(string viewName)
        {
            return Task.CompletedTask;
        }

        public virtual Task QueueEventAsync(string category, string action, string? label = null, long value = 0)
        {
            return Task.CompletedTask;
        }

        public virtual Task QueueTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1)
        {
            return Task.CompletedTask;
        }

        public virtual Task QueueTimingAsync(TimeSpan time, string category, string variable, string? label = null)
        {
            return Task.CompletedTask;
        }

        public virtual Task SendAsync()
        {
            return Task.CompletedTask;
        }
    }
}
