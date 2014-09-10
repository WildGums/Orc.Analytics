// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalyticsAuditor.cs" company="CatenaLogic">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Analytics.Auditors
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Catel.MVVM;
    using Catel.MVVM.Auditing;
    using Services;

    public class AnalyticsAuditor : AuditorBase
    {
        #region Fields
        private readonly IGoogleAnalyticsService _analyticsService;

        private readonly Dictionary<int, DateTime> _viewModelCreationTimes = new Dictionary<int, DateTime>();
        #endregion

        #region Constructors
        public AnalyticsAuditor(IGoogleAnalyticsService analyticsService)
        {
            Argument.IsNotNull(() => analyticsService);

            _analyticsService = analyticsService;
        }
        #endregion

        #region Methods
        public override void OnCommandExecuted(IViewModel viewModel, string commandName, ICatelCommand command, object commandParameter)
        {
            base.OnCommandExecuted(viewModel, commandName, command, commandParameter);

            _analyticsService.SendCommand(viewModel.GetType().Name, commandName);
        }

        public override void OnViewModelCreated(IViewModel viewModel)
        {
            base.OnViewModelCreated(viewModel);

            _viewModelCreationTimes[viewModel.UniqueIdentifier] = DateTime.Now;

            _analyticsService.SendViewModelCreated(viewModel.GetType().FullName);
        }

        public override void OnViewModelClosed(IViewModel viewModel)
        {
            base.OnViewModelClosed(viewModel);

            var lifetime = DateTime.Now.Subtract(_viewModelCreationTimes[viewModel.UniqueIdentifier]);

            _analyticsService.SendViewModelClosed(viewModel.GetType().FullName, lifetime);
        }
        #endregion
    }
}