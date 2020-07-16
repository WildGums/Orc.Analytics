// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAnalyticsService _analyticsService;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel(IAnalyticsService analyticsService)
        {
            Argument.IsNotNull(() => analyticsService);

            _analyticsService = analyticsService;

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
            _analyticsService.AccountId = AccountId;
        }
        #endregion
    }
}