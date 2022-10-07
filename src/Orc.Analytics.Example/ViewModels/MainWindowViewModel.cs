namespace Orc.Analytics.Example.ViewModels
{
    using System;
    using Catel.Logging;
    using Catel.MVVM;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAnalyticsService _analyticsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IAnalyticsService analyticsService)
        {
            ArgumentNullException.ThrowIfNull(analyticsService);

            _analyticsService = analyticsService;

            AccountId = "UA-54670241-1";
        }

        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Orc.Analytics.Example"; }
        }

        public string AccountId { get; set; }


        private void OnAccountIdChanged()
        {
            //_analyticsService.AccountId = AccountId;
        }
    }
}
