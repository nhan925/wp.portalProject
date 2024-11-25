using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Identity.Client;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        this.InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(null);
        this.SetWindowSize(1000, 600);
        this.SetIsResizable(false);
        this.CenterOnScreen();
        this.SetIsMaximizable(false);
        LoginFrame.Navigate(typeof(LoginWelcomePage), this);
    }

    public LoginWindow(LaunchActivatedEventArgs args)
    {
        this.InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(null);
        this.SetWindowSize(1000, 600);
        this.SetIsResizable(false);
        this.CenterOnScreen();
        this.SetIsMaximizable(false);
        LoginFrame.Navigate(typeof(LoginWelcomePage), this);
        LoginWindowsViewModel.Instance.LaunchArgs = args;
    }

    public void NavigateToConfirmUserNamePage()
    {
        LoginFrame.Navigate(typeof(LoginForgotPasswordPage01), this);
    }

    public void NavigateToConfirmOTPPage()    
    {
        LoginFrame.Navigate(typeof(LoginForgotPasswordPage02), this);
    }

    public void NavigateToCreateNewPasswordPage()
    {
        LoginFrame.Navigate(typeof(LoginForgotPasswordPage03), this);
    }

    public void NavigateToWelcomePage()
    {
        LoginFrame.Navigate(typeof(LoginWelcomePage), this);
    }
}
