using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class RequestDetailPage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    public RequestDetailViewModel ViewModel
    {
        get;
    }

    public RequestDetailPage()
    {
        ViewModel = App.GetService<RequestDetailViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        ViewModel.Request = e.Parameter as InformationsForRequest_RequestRow;
        await Task.Delay(10);

        ViewModel.formatRequestContent();
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }

    private void ResendRequestButton_Click(object sender, RoutedEventArgs e)
    {
        var message = ViewModel.resendRequest();
        var title = (message == resourceLoader.GetString("RequestDetailPage_ResendRequestSuccess/Text"))
                    ? resourceLoader.GetString("App_Success/Text")
                    : resourceLoader.GetString("App_Error/Text");
        ShowResendMessage(message,title);
    }

    private void CancelledRequestButton_Click(object sender, RoutedEventArgs e)
    {
        var message = ViewModel.cancelledRequest();
        var title = (message == resourceLoader.GetString("RequestDetailPage_CancelledRequestSuccess/Text"))
                    ? resourceLoader.GetString("App_Success/Text")
                    : resourceLoader.GetString("App_Error/Text");
        ShowResendMessage(message, title);
    }

    private async void ShowResendMessage(string message,string title)
    {
        ContentDialog errorDialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            XamlRoot = this.Content.XamlRoot,
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };

        await errorDialog.ShowAsync();
    }

}
