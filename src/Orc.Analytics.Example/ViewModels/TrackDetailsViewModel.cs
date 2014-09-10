// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackDetailsViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Example.ViewModels
{
    using Catel;
    using Catel.MVVM;

    /// <summary>
    /// Class TrackDetailsViewModel.
    /// </summary>
    public class TrackDetailsViewModel : ViewModelBase
    {
        private readonly IGoogleAnalyticsService _googleAnalyticsService;

        public TrackDetailsViewModel(IGoogleAnalyticsService googleAnalyticsService)
        {
            Argument.IsNotNull(() => googleAnalyticsService);

            _googleAnalyticsService = googleAnalyticsService;

            Send = new Command(OnSendExecute, OnSendCanExecute);

            Category = "category";
            Action = "action";
        }

        public string Category { get; set; }

        public string Action { get; set; }

        #region Commands
        public Command Send { get; private set; }

        private bool OnSendCanExecute()
        {
            if (string.IsNullOrWhiteSpace(Category))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Action))
            {
                return false;
            }

            return true;
        }

        private async void OnSendExecute()
        {
            await _googleAnalyticsService.SendEvent(Category, Action);
        }
        #endregion
    }
}