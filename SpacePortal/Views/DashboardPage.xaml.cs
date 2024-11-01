using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Services;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.Charts;
using Windows.Devices.Input;

namespace SpacePortal.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(5);

        LineChartLoadingOverlay.Visibility = Visibility.Collapsed;
        PieChartLoadingOverlay.Visibility = Visibility.Collapsed;

        LineChart.Visibility = Visibility.Visible;
        PieChart.Visibility = Visibility.Visible;
    }

    private void NavigateTo(string ViewModelFullName)
    {
        var navigationService = App.GetService<INavigationService>();
        var pageKey = ViewModelFullName;
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey);
        }
    }

    private void InforButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(InformationViewModel).FullName ?? "SpacePortal.ViewModels.InformationViewModel");
    }

    private void ScheduleButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(ScheduleViewModel).FullName ?? "SpacePortal.ViewModels.ScheduleViewModel");
    }

    private void FeeButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(TuitionFeeViewModel).FullName ?? "SpacePortal.ViewModels.TuitionFeeViewModel");
    }

    private void GradeButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(GradesViewModel).FullName ?? "SpacePortal.ViewModels.GradesViewModel");
    }

    private void RegistrationButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(CoursesRegistrationViewModel).FullName ?? "SpacePortal.ViewModels.CoursesRegistrationViewModel");
    }

    private void ScholarshipButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        NavigateTo(typeof(ScholarshipViewModel).FullName ?? "SpacePortal.ViewModels.ScholarshipViewModel");
    }
}
