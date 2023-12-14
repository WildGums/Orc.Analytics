namespace Orc.Analytics.Example.ViewModels;

using System;
using System.Threading.Tasks;
using Catel;
using Catel.MVVM;

/// <summary>
/// Class TrackDetailsViewModel.
/// </summary>
public class TrackDetailsViewModel : ViewModelBase
{
    private readonly IAnalyticsService _analyticsService;

    public TrackDetailsViewModel(IAnalyticsService analyticsService)
    {
        ArgumentNullException.ThrowIfNull(analyticsService);

        _analyticsService = analyticsService;

        Send = new TaskCommand(OnSendExecuteAsync, OnSendCanExecute);

        Category = "category";
        Action = "action";
    }

    public string Category { get; set; }

    public string Action { get; set; }

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
        await _analyticsService.QueueEventAsync(Category, Action);
    }
}
