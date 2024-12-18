using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class TuitionFeePage : Page
{
    public TuitionFeeViewModel ViewModel
    {
        get;
    }

    public TuitionFeePage()
    {
        ViewModel = App.GetService<TuitionFeeViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        ListViewLoadingOverlay.Visibility = Visibility.Collapsed;
        PeriodsListView.Visibility = Visibility.Visible;
    }

    private void TuitionFee_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = sender as Button;
        var context = button?.DataContext as TuitionFeeListInformations;

        Tuple<int, string, int, int, int> info = new(context.semesterID, context.year, context.semester, context.totalCourse, context.totalTuitionFee);

        var navigationService = App.GetService<INavigationService>();
        var pageKey = typeof(TuitionFeeDetailViewModel).FullName ?? "SpacePortal.ViewModels.TuitionFeeDetailViewModel";
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey, info);
        }
    }
}

