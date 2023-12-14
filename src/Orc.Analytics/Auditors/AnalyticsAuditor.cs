namespace Orc.Analytics.Auditors;

using System;
using System.Collections.Generic;
using Catel.MVVM;
using Catel.MVVM.Auditing;
using Catel.Reflection;

public class AnalyticsAuditor : AuditorBase
{
    private readonly IAnalyticsService _analyticsService;

    private readonly Dictionary<int, DateTime> _viewModelCreationTimes = new();

    public AnalyticsAuditor(IAnalyticsService analyticsService)
    {
        ArgumentNullException.ThrowIfNull(analyticsService);

        _analyticsService = analyticsService;
    }

    public override async void OnCommandExecuted(IViewModel? viewModel, string? commandName, ICatelCommand command, object? commandParameter)
    {
        base.OnCommandExecuted(viewModel, commandName, command, commandParameter);

        var viewModelName = viewModel is not null ? viewModel.GetType().Name : string.Empty;
        var finalCommandName = commandName ?? string.Empty;

        await _analyticsService.QueueCommandAsync(viewModelName, finalCommandName);
    }

    public override async void OnViewModelCreated(IViewModel viewModel)
    {
        base.OnViewModelCreated(viewModel);

        _viewModelCreationTimes[viewModel.UniqueIdentifier] = DateTime.Now;

        await _analyticsService.QueueViewModelCreatedAsync(viewModel.GetType().GetSafeFullName());
    }

    public override async void OnViewModelClosed(IViewModel viewModel)
    {
        base.OnViewModelClosed(viewModel);

        if (!_viewModelCreationTimes.ContainsKey(viewModel.UniqueIdentifier))
        {
            return;
        }

        var lifetime = DateTime.Now.Subtract(_viewModelCreationTimes[viewModel.UniqueIdentifier]);

        await _analyticsService.QueueViewModelClosedAsync(viewModel.GetType().GetSafeFullName(), lifetime);
    }
}
