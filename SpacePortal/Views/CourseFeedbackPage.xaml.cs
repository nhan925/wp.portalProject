
namespace SpacePortal.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;


public sealed partial class CourseFeedbackPage : Page
{
    public CourseFeedbackViewModel ViewModel
    {
        get;
    }

    public CourseFeedbackPage()
    {
        ViewModel = App.GetService<CourseFeedbackViewModel>();
        InitializeComponent();
    }

    private void CourseFeedback_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var button = sender as Button;
        var context = button?.DataContext as CourseFeedbackListInformations;

        Tuple<string, string, string, string> info = new(context.courseName,context.courseID, context.teacherName, context.classID);

        var navigationService = App.GetService<INavigationService>();
        var pageKey = typeof(CourseFeedbackDetailViewModel).FullName ?? "SpacePortal.ViewModels.CourseFeedbackDetailViewModel";
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey, info);
        }
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        ListViewLoadingOverlay.Visibility = Visibility.Collapsed;
        PeriodsListView.Visibility = Visibility.Visible;
    }

}
