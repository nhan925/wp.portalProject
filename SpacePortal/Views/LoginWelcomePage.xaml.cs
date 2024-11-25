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
using Windows.Foundation;
using Windows.Foundation.Collections;
using SpacePortal.ViewModels;
using System.Diagnostics;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginWelcomePage : Page
{

    ResourceLoader resourceLoader = new ResourceLoader();
    public LoginWindowsViewModel ViewModel { get; set; }
    private LoginWindow ParentWindow;

    public LoginWelcomePage()
    {
        this.InitializeComponent(); 
        this.RequestedTheme = ElementTheme.Light;
        ViewModel = LoginWindowsViewModel.Instance;
        if (ViewModel.FirstTime)
        {
            ViewModel.LoadLoginInfoFromLocal();
            ViewModel.FirstTime = false;
        }
    }


    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is LoginWindow parentWindow)
        {
            ParentWindow = parentWindow;
        }

        ViewModel.ResetInstance();
    }

    private async void LoginWithRawButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.CheckLoginWithRawInformation())
        {
            App.MainWindow = new MainWindow();
            await App.GetService<IActivationService>().ActivateAsync(LoginWindowsViewModel.Instance.LaunchArgs);
            ParentWindow.Close();
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("Login_Error_UserNameAndPasword/Text"),
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            await dialog.ShowAsync();
        }
    }

    private async void LoginWithOulookButton_Click(object sender, RoutedEventArgs e)
    {
        var result = await ViewModel.LoginWithOutlook();

        if (result)
        {
            App.MainWindow = new MainWindow();
            await App.GetService<IActivationService>().ActivateAsync(LoginWindowsViewModel.Instance.LaunchArgs);
            ParentWindow.Close();
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = resourceLoader.GetString("App_Error/Text"),
                Content = resourceLoader.GetString("Login_Error_UserNameAndPasword/Text"),
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                XamlRoot = this.XamlRoot,
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            _ = dialog.ShowAsync();
        }
    }

    private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
    {
        ParentWindow.NavigateToConfirmUserNamePage();
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        usernameTextBox.Select(ViewModel.UserName.Length, 0);
    }
}
