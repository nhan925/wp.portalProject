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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LoginWelcomePage : Page
{


    public LoginWindowsViewModel ViewModel { get; set; }
    private LoginWindow ParentWindow;

    public LoginWelcomePage()
    {
        this.InitializeComponent(); 
        ViewModel = LoginWindowsViewModel.Instance;

    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is LoginWindow parentWindow)
        {
            ParentWindow = parentWindow;
        }
    }


    private void LoginWithRawButton_Click(object sender, RoutedEventArgs e)
    {
        
        if (ViewModel.CheckLoginWithRawInformation())
        {
            //Navigate To SpacePortal
        }
        else
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Đăng nhập thất bại",
                Content = "Tên tài khoản hoặc mật khẩu không chính xác",
                CloseButtonText = "Đóng",
                XamlRoot = this.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
    }

    private void LoginWithOulookButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
    {
        ParentWindow.NavigateToConfirmUserNamePage();
    }

    private void RememberMeCheckBox_Click(object sender, RoutedEventArgs e)
    {

    }
}
