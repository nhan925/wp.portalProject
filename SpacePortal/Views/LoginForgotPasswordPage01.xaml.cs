using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginForgotPasswordPage01 : Page
{
    ResourceLoader resourceLoader = new ResourceLoader();
    public LoginWindowsViewModel ViewModel { get; set; }
    private LoginWindow ParentWindow;

    public LoginForgotPasswordPage01()
    {
        this.InitializeComponent();
        this.RequestedTheme = ElementTheme.Light;
        ViewModel = LoginWindowsViewModel.Instance;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is LoginWindow parentWindow)
        {
            ParentWindow = parentWindow;
        }

        (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();
    }


    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        ParentWindow.NavigateToWelcomePage();
    }

    private async void ConfirmUserNameButton_Click(object sender, RoutedEventArgs e)
    {
        (App.LoginWindow as LoginWindow)?.ShowLoadingOverlay();
        await Task.Delay(10);

        if (ViewModel.CheckUserNameAndSendOTP())
        {
            ParentWindow.NavigateToConfirmOTPPage();
            ViewModel.SetEmailNotificationCaption();
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("Login_Error_ConfirmUserName/Text"),
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            dialog.CloseButtonClick += (s, e) => (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();
            _ = dialog.ShowAsync();
        }
    }
}
