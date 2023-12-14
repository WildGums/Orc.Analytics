using Catel.IoC;
using Catel.Services;
using Orc.Analytics;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        var serviceLocator = ServiceLocator.Default;

        serviceLocator.RegisterType<IUserIdService, UserIdService>();
        serviceLocator.RegisterType<IAnalyticsService, EmptyAnalyticsService>();

        var languageService = serviceLocator.ResolveRequiredType<ILanguageService>();
        languageService.RegisterLanguageSource(new LanguageResourceSource("Orc.Analytics", "Orc.Analytics.Properties", "Resources"));
    }
}
