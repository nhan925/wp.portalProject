using System.Text.RegularExpressions;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using SpacePortal.Contracts.Services;
using SpacePortal.Helpers;
using SpacePortal.ViewModels;

using Windows.System;
using WinUIEx.Messaging;

namespace SpacePortal.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        // A custom title bar is required for full window theme and Mica support.
        // https://docs.microsoft.com/windows/apps/develop/title-bar?tabs=winui3#full-customization
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();
    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }

    private void DetailButton_Click(object sender, RoutedEventArgs e)
    {
        App.GetService<INavigationService>().NavigateTo(typeof(InformationViewModel).FullName 
            ?? "SpacePortal.ViewModels.InformationViewModel");
        AccountAvatarButton.Flyout.Hide();
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        LoginWindowsViewModel.Instance.ClearLoginInfo();
        App.LoginWindow = new LoginWindow(LoginWindowsViewModel.Instance.LaunchArgs);
        App.LoginWindow.Activate();
        App.MainWindow.Close();
    }

    private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
    {
        AccountAvatarButton.Flyout.Hide();

        var r = new ResourceLoader();
        var stackPanel = new StackPanel()
        {
            Spacing = 12,
            Width = 450,
            Height = 260,
        };

        var currentPasswordTextBlock = new TextBlock()
        {
            Text = r.GetString("ChangePasswordDialog_CurrentPassword")
        };
        var newPasswordTextBlock = new TextBlock()
        {
            Text = r.GetString("ChangePasswordDialog_NewPassword")
        };
        var confirmNewPasswordTextBlock = new TextBlock()
        {
            Text = r.GetString("ChangePasswordDialog_ConfirmNewPassword")
        };
        var alertTextBlock = new TextBlock()
        {
            Text = String.Empty,
            Foreground = new SolidColorBrush(Colors.Red),
            FontSize = 12,
            Opacity = 0.8,
            TextWrapping = TextWrapping.WrapWholeWords
        };

        var currentPasswordBox = new PasswordBox();
        var newPasswordBox = new PasswordBox();
        var confirmNewPasswordBox = new PasswordBox();

        stackPanel.Children.Add(currentPasswordTextBlock);
        stackPanel.Children.Add(currentPasswordBox);
        stackPanel.Children.Add(newPasswordTextBlock);
        stackPanel.Children.Add(newPasswordBox);
        stackPanel.Children.Add(confirmNewPasswordTextBlock);
        stackPanel.Children.Add(confirmNewPasswordBox);
        stackPanel.Children.Add(alertTextBlock);

        var dialog = new ContentDialog()
        {
            Title = r.GetString("ChangePasswordDialog_Title"),
            Content = stackPanel,
            CloseButtonText = r.GetString("App_Close/Text"),
            PrimaryButtonText = r.GetString("ChangePasswordDialog_ConfirmButton"),
            RequestedTheme = RequestedTheme = App.GetService<IThemeSelectorService>().Theme,
            XamlRoot = this.XamlRoot,
            PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
        };

        dialog.PrimaryButtonClick += (s, e) =>
        {
            alertTextBlock.Text = String.Empty;

            if (String.IsNullOrEmpty(currentPasswordBox.Password) || String.IsNullOrEmpty(newPasswordBox.Password) || String.IsNullOrEmpty(confirmNewPasswordBox.Password))
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_EmptyField");
                e.Cancel = true;
                return;
            }

            if (newPasswordBox.Password != confirmNewPasswordBox.Password)
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_PasswordNotMatch");
                e.Cancel = true;
                return;
            }

            Regex regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            if (!regex.IsMatch(newPasswordBox.Password))
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_PasswordNotValid");
                e.Cancel = true;
                return;
            }

            var code = ViewModel.ChangePassword(currentPasswordBox.Password, newPasswordBox.Password);
            if (code == "SUCCESS")
            {
                var toastBuilder = new AppNotificationBuilder()
                .AddText(r.GetString("ChangePasswordDialog_Title"))
                .AddText(r.GetString("ChangePasswordDialog_Success"));

                var toast = toastBuilder.BuildNotification();
                AppNotificationManager.Default.Show(toast);

                LoginWindowsViewModel.Instance.encodingPassword(newPasswordBox.Password);
            }
            else if (code == "WRONG")
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_WrongPassword");
                e.Cancel = true;
            }
            else if (code == "DUPLICATE")
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_DuplicatePassword");
                e.Cancel = true;
            }
            else
            {
                alertTextBlock.Text = r.GetString("ChangePasswordDialog_Error");
                e.Cancel = true;
            }
        };

        await dialog.ShowAsync();
    }
}
