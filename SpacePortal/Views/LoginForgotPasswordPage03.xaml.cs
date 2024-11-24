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
using Microsoft.Windows.ApplicationModel.Resources;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginForgotPasswordPage03 : Page
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        public LoginWindowsViewModel ViewModel {get; set;}
        private LoginWindow ParentWindow;

        public LoginForgotPasswordPage03()
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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var message = ViewModel.CheckPasswordAndConfirmNewPassword();
            if(message == "success")
            {
                if (ViewModel.UpdateNewPassword())
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = resourceLoader.GetString("App_Title_Successful/Text"),
                        Content = resourceLoader.GetString("Login_ResetPassword_Complete/Text"),
                        CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                        XamlRoot = this.XamlRoot
                    };
                    _ = dialog.ShowAsync();

                    dialog.CloseButtonClick += (s, e) =>
                    {
                        ParentWindow.NavigateToWelcomePage();
                    };
                }
             
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = resourceLoader.GetString("App_Error/Text"),
                    Content = message,
                    CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
