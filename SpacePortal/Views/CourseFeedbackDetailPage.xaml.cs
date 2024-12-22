using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.ViewModels;
using WinUIEx.Messaging;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Diagnostics;
namespace SpacePortal.Views;

public sealed partial class CourseFeedbackDetailPage : Page
{
    public CourseFeedbackDetailViewModel ViewModel { get; }

    public CourseFeedbackDetailPage()
    {
        ViewModel = App.GetService<CourseFeedbackDetailViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);
        if (e.Parameter is Tuple<string, string, string, string> info)
        {
            ViewModel.LoadInformations(info.Item1, info.Item2, info.Item3, info.Item4);
        }

        ViewModel.LoadCourseQuestion();
        ViewModel.LoadTeacherQuestion();
    }

    private void GoBackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }

    private async void Complete_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var result = ViewModel.CheckEmptyField();

        if (!result)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            ContentDialog errorDialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("CourseFeedbackDetail_EmptyFieldError/Text"),
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.Content.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };

            await errorDialog.ShowAsync();
        }
        else
        {
            if (ViewModel.UploadFeedbackToDatabase())
            {
                ResourceLoader resourceLoader = new ResourceLoader();
                ContentDialog completeDialog = new ContentDialog
                {
                    Title = resourceLoader.GetString("App_Success/Text"),
                    Content = resourceLoader.GetString("CourseFeedbackDetail_CompleteMessage/Text"),
                    CloseButtonText = resourceLoader.GetString("App_Yes/Text"),
                    XamlRoot = this.Content.XamlRoot,
                    RequestedTheme = App.GetService<IThemeSelectorService>().Theme
                };
                await completeDialog.ShowAsync();

                var navigationService = App.GetService<INavigationService>();
                navigationService.GoBack();
            }
            else
            {
                //TODO: Handle error
            }
        }
    }
}
