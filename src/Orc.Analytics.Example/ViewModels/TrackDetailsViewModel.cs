// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackDetailsViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Example.ViewModels
{
    using System.Threading.Tasks;
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

            Send = new TaskCommand(OnSendExecuteAsync, OnSendCanExecute);

            Category = "category";
            Action = "action";
        }

        public string Category { get; set; }

        public string Action { get; set; }

        #region Commands
        public TaskCommand Send { get; private set; }

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

        private async Task OnSendExecuteAsync()
        {
            await _googleAnalyticsService.SendEventAsync(Category, Action);
        }
        #endregion
    }
}