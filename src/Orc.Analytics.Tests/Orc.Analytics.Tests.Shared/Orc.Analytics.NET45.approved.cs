[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.5", FrameworkDisplayName=".NET Framework 4.5")]


public class static LoadAssembliesOnStartup { }
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.Analytics.Auditors
{
    
    public class AnalyticsAuditor : Catel.MVVM.Auditing.AuditorBase
    {
        public AnalyticsAuditor(Orc.Analytics.IAnalyticsService analyticsService) { }
        public override void OnCommandExecuted(Catel.MVVM.IViewModel viewModel, string commandName, Catel.MVVM.ICatelCommand command, object commandParameter) { }
        public override void OnViewModelClosed(Catel.MVVM.IViewModel viewModel) { }
        public override void OnViewModelCreated(Catel.MVVM.IViewModel viewModel) { }
    }
}
namespace Orc.Analytics
{
    
    public class static FrameworkElementExtensions
    {
        public static System.Threading.Tasks.Task TrackViewForAnalyticsAsync(this System.Windows.FrameworkElement frameworkElement) { }
    }
    public class GoogleAnalyticsService : Orc.Analytics.IAnalyticsService, Orc.Analytics.IGoogleAnalyticsService
    {
        public GoogleAnalyticsService(Orc.Analytics.IUserIdService userIdService) { }
        public string AccountId { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public bool IsEnabled { get; set; }
        public string UserId { get; set; }
        public System.Threading.Tasks.Task InvokeAsync(System.Action action) { }
        public System.Threading.Tasks.Task SendEventAsync(string category, string action, string label = null, long value = 0) { }
        public System.Threading.Tasks.Task SendTimingAsync(System.TimeSpan time, string category, string variable, string label = "") { }
        public System.Threading.Tasks.Task SendTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1) { }
        public System.Threading.Tasks.Task SendViewAsync(string viewName) { }
    }
    public interface IAnalyticsService
    {
        string AccountId { get; set; }
        string AppName { get; set; }
        string AppVersion { get; set; }
        bool IsEnabled { get; set; }
        string UserId { get; set; }
        System.Threading.Tasks.Task SendEventAsync(string category, string action, string label = null, long value = 0);
        System.Threading.Tasks.Task SendTimingAsync(System.TimeSpan time, string category, string variable, string label = "");
        System.Threading.Tasks.Task SendTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1);
        System.Threading.Tasks.Task SendViewAsync(string viewName);
    }
    public class static IAnalyticsServiceExtensions
    {
        public static System.Threading.Tasks.Task ExecuteAndTrackAsync(this Orc.Analytics.IAnalyticsService service, System.Func<System.Threading.Tasks.Task> func, string category, string variable) { }
        public static System.Threading.Tasks.Task<T> ExecuteAndTrackWithResultAsync<T>(this Orc.Analytics.IAnalyticsService service, System.Func<System.Threading.Tasks.Task<T>> func, string category, string variable) { }
        public static System.Threading.Tasks.Task SendCommandAsync(this Orc.Analytics.IAnalyticsService googleAnalytics, string viewModelName, string commandName) { }
        public static System.Threading.Tasks.Task SendViewModelClosedAsync(this Orc.Analytics.IAnalyticsService googleAnalytics, string viewModel, System.TimeSpan duration) { }
        public static System.Threading.Tasks.Task SendViewModelCreatedAsync(this Orc.Analytics.IAnalyticsService googleAnalytics, string viewModel) { }
    }
    [ObsoleteExAttribute(Message="Use non-brand specific version", RemoveInVersion="3.0", ReplacementTypeOrMember="IAnalyticsService", TreatAsErrorFromVersion="2.0")]
    public interface IGoogleAnalyticsService
    {
        string AccountId { get; set; }
        string AppName { get; set; }
        string AppVersion { get; set; }
        bool IsEnabled { get; set; }
        string UserId { get; set; }
        System.Threading.Tasks.Task SendEventAsync(string category, string action, string label = null, long value = 0);
        System.Threading.Tasks.Task SendTimingAsync(System.TimeSpan time, string category, string variable, string label = "");
        System.Threading.Tasks.Task SendTransactionAsync(string sku, string name, string transactionId, long costPerProduct, int quantity = 1);
        System.Threading.Tasks.Task SendViewAsync(string viewName);
    }
    [ObsoleteExAttribute(Message="Use non-brand specific version", RemoveInVersion="3.0", ReplacementTypeOrMember="IAnalyticsServiceExtensions", TreatAsErrorFromVersion="2.0")]
    public class static IGoogleAnalyticsServiceExtensions
    {
        public static System.Threading.Tasks.Task ExecuteAndTrackAsync(this Orc.Analytics.IGoogleAnalyticsService service, System.Func<System.Threading.Tasks.Task> func, string category, string variable) { }
        public static System.Threading.Tasks.Task<T> ExecuteAndTrackWithResultAsync<T>(this Orc.Analytics.IGoogleAnalyticsService service, System.Func<System.Threading.Tasks.Task<T>> func, string category, string variable) { }
        public static System.Threading.Tasks.Task SendCommandAsync(this Orc.Analytics.IGoogleAnalyticsService googleAnalytics, string viewModelName, string commandName) { }
        public static System.Threading.Tasks.Task SendViewModelClosedAsync(this Orc.Analytics.IGoogleAnalyticsService googleAnalytics, string viewModel, System.TimeSpan duration) { }
        public static System.Threading.Tasks.Task SendViewModelCreatedAsync(this Orc.Analytics.IGoogleAnalyticsService googleAnalytics, string viewModel) { }
    }
    public interface IUserIdService
    {
        string GetUserId();
    }
    public class UserIdService : Orc.Analytics.IUserIdService
    {
        public UserIdService(Orc.SystemInfo.ISystemIdentificationService systemIdentificationService) { }
        public string GetUserId() { }
    }
}