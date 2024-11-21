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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginForgotPasswordPage03 : Page
    {
        public LoginWindowsViewModel ViewModel
        {
            get; set;
        }
        private LoginWindow ParentWindow;

        public LoginForgotPasswordPage03()
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var message = ViewModel.CheckPasswordAndConfirmNewPassword();
            if(message == "success")
            {
                ViewModel.UpdateNewPassword();
                //Navigate to SpacePortal
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Đổi mật khẩu thất bại",
                    Content = message,
                    CloseButtonText = "OK"
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
