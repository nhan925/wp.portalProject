using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class CoursesRegistrationPage : Page
{
    public CoursesRegistrationViewModel ViewModel
    {
        get;
    }

    public CoursesRegistrationPage()
    {
        ViewModel = App.GetService<CoursesRegistrationViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        ListViewLoadingOverlay.Visibility = Visibility.Collapsed;
        PeriodsListView.Visibility = Visibility.Visible;
    }

    private void Period_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var context = button?.DataContext as CoursesRegistrationPeriodInformation;

        KeyValuePair<int, string> info = new(context.Id, context.Title);

        var navigationService = App.GetService<INavigationService>();
        var pageKey = typeof(ChooseCoursesViewModel).FullName ?? "SpacePortal.ViewModels.ChooseCoursesViewModel";
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey, info);
        }
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e)
    {
        PeriodsListView.Opacity = 0.5;
        ListViewLoadingOverlay.Visibility = Visibility.Visible;
        await Task.Delay(10);
        ViewModel.Init();
        PeriodsListView.Opacity = 1;
        ListViewLoadingOverlay.Visibility = Visibility.Collapsed;
    }
}
