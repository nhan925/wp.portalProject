using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;

namespace SpacePortal.Views;

public sealed partial class InformationPage : Page
{
    

    public InformationViewModel ViewModel
    {
        get;
    }

    public InformationPage()
    {
        ViewModel = App.GetService<InformationViewModel>();
        InitializeComponent();
    }


    private void ChangeContact_Click(object sender, RoutedEventArgs e)
    {

        ViewModel.ChangeContact();  
    }

    private void AcceptChanges_Click(object sender, RoutedEventArgs e)
    {

       
        var message = ViewModel.CheckAcceptChanges();

        if (message == "Success")
        {
            ViewModel.AcceptChanges();

            ViewModel.Save();
        }
        else
        {
            ShowErrorMessage(message);
        }
    }


    private async void ShowErrorMessage(string message)
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        ContentDialog errorDialog = new ContentDialog
        {
            Title = resourceLoader.GetString("App_Error/Text"),
            Content = message,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            XamlRoot = this.Content.XamlRoot,
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };

        await errorDialog.ShowAsync();
    }

    private void CancelChanges_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.CancelChanges();
    }

    private void UploadAvatar_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.UploadAvatar();

        
    }

    private void RemoveAvatar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RemoveAvatar();
        Avatar.Flyout.Hide();
    }

 
}
