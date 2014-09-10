// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Example.ViewModels
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IGoogleAnalyticsService _googleAnalyticsService;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IGoogleAnalyticsService googleAnalyticsService)
        {
            Argument.IsNotNull(() => googleAnalyticsService);

            _googleAnalyticsService = googleAnalyticsService;

            AccountId = "UA-54670241-1";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Orc.Analytics.Example"; }
        }

        public string AccountId { get; set; }
        #endregion

        #region Commands

        #endregion

        #region Methods
        private void OnAccountIdChanged()
        {
            _googleAnalyticsService.AccountId = AccountId;
        }
        #endregion
    }
}