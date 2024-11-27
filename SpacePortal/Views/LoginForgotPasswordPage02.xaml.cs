using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;
using Windows.Media.Core;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginForgotPasswordPage02 : Page
{
    ResourceLoader resourceLoader = new ResourceLoader();
    public LoginWindowsViewModel ViewModel { get; set; }
    private LoginWindow? ParentWindow;
    private readonly DispatcherTimer timer = new();


    public LoginForgotPasswordPage02()
    {
        this.InitializeComponent();
        this.RequestedTheme = ElementTheme.Light;
        ViewModel = LoginWindowsViewModel.Instance;
    }

    private void Timer_Tick(object? sender, object? e)
    {
        if (ViewModel.CountDownTime > 0)
        {
            ViewModel.CountDownTime--;
            ViewModel.CountDownNotificationCaption = $"{resourceLoader.GetString("Login_OTPExpiredAfterText")} {ViewModel.CountDownTime} {resourceLoader.GetString("Login_Second")}";
        }
        else
        {
            ViewModel.CountDownNotificationCaption = resourceLoader.GetString("Login_OTPExpiredNoti");
            timer.Stop();
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is LoginWindow parentWindow)
        {
            ParentWindow = parentWindow;
        }

        ViewModel.ConfirmOTP = "";
        ViewModel.CountDownTime = 60;
        ViewModel.CountDownNotificationCaption = $"{resourceLoader.GetString("Login_OTPExpiredAfterText")} {ViewModel.CountDownTime} {resourceLoader.GetString("Login_Second")}";
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
        (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();

    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        timer.Stop();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        ParentWindow?.NavigateToConfirmUserNamePage();
    }

    private async void RequestOTPButton_Click(object sender, RoutedEventArgs e)
    {
        (App.LoginWindow as LoginWindow)?.ShowLoadingOverlay();
        await Task.Delay(10);

        if (ViewModel.CheckUserNameAndSendOTP())
        {
            timer.Stop();
            ViewModel.ConfirmOTP = "";
            ViewModel.CountDownTime = 60;
            ViewModel.CountDownNotificationCaption = $"{resourceLoader.GetString("Login_OTPExpiredAfterText")} {ViewModel.CountDownTime} {resourceLoader.GetString("Login_Second")}";
            timer.Start();
            (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("Login_Error_RequestOTP/Text"),   
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            dialog.CloseButtonClick += (s, e) => (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();
            _ = dialog.ShowAsync();
        }
    }

    private async void ConfirmOTPButton_Click(object sender, RoutedEventArgs e)
    {
        (App.LoginWindow as LoginWindow)?.ShowLoadingOverlay();
        await Task.Delay(10);

        if (ViewModel.ValidateOTP())
        {
            ParentWindow?.NavigateToCreateNewPasswordPage();
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("Login_Error_OTPCode/Text"),
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            dialog.CloseButtonClick += (s, e) => (App.LoginWindow as LoginWindow)?.HideLoadingOverlay();
            _ = dialog.ShowAsync();
        }
    }
}
