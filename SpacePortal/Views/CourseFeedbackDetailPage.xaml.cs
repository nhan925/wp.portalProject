using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class CourseFeedbackDetailPage : Page
{
    public CourseFeedbackDetailViewModel ViewModel
    {
        get;
    }

    public CourseFeedbackDetailPage()
    {
        ViewModel = App.GetService<CourseFeedbackDetailViewModel>();
        InitializeComponent();
    }
    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is Tuple<string, string, string, string> info)
        {
            await Task.Delay(10);
            ViewModel.LoadInformations(info.Item1, info.Item2, info.Item3, info.Item4);
        }

        ViewModel.LoadCourseQuesntion();
        ViewModel.LoadTeacherQuesntion();
        //CourseFeeListGrid.Visibility = Visibility.Visible;
        //CourseFeeListLoading.Visibility = Visibility.Collapsed;
    }

    private void GoBackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }

    private void Complete_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //TODO
    }
}
