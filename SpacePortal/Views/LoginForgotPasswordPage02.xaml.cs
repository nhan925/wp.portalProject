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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginForgotPasswordPage02 : Page
    {
        private LoginWindow ParentWindow;

        public LoginForgotPasswordPage02()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is LoginWindow parentWindow)
            {
                ParentWindow = parentWindow;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.NavigateToConfirmUserNamePage();
        }

        private void RequestOTPButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {

            ParentWindow.NavigateToCreateNewPasswordPage();
        }
    }
}
