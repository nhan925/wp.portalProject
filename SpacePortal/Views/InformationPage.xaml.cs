using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Views;

public sealed partial class InformationPage : Page
{
    private ResourceLoader resourceLoader = new ResourceLoader();

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

        if (ViewModel.IsEditing)
        {
            ViewModel.EditPersonalEmail = ViewModel.informations.PersonalEmail;
            ViewModel.EditPhoneNumber = ViewModel.informations.PhoneNumber;
            ViewModel.EditAddress = ViewModel.informations.Address;

            ViewModel.IsEditing = !ViewModel.IsEditing;
        }
        else
        {
            ViewModel.IsEditing = !ViewModel.IsEditing;
        }
    }

    private void AcceptChanges_Click(object sender, RoutedEventArgs e)
    {
        string email = ViewModel.EditPersonalEmail;
        string phoneNumber = ViewModel.EditPhoneNumber;
        string address = ViewModel.EditAddress;

        
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(address))
        {
            ShowErrorMessage(resourceLoader.GetString("Information_Error_NotEmpty/Text"));
            return;
        }
        
        if (phoneNumber.Length != 10 || !phoneNumber.All(char.IsDigit))
        {
            ShowErrorMessage(resourceLoader.GetString("Information_Error_PhoneNumber/Text"));
            return;
        }
        
        ViewModel.AcceptChanges();
    }


    private async void ShowErrorMessage(string message)
    {
        ContentDialog errorDialog = new ContentDialog
        {
            Title = resourceLoader.GetString("App_Error/Text"),
            Content = message,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            XamlRoot = this.Content.XamlRoot
        };

        await errorDialog.ShowAsync();
    }

    private void CancelChanges_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.CancelChanges();
    }

    private async void UploadAvatar_Click(object sender, RoutedEventArgs e)
    {
        FileOpenPicker fileOpenPicker = new()
        {
            ViewMode = PickerViewMode.Thumbnail,
            FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif" },
        };

        nint windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);

        StorageFile file = await fileOpenPicker.PickSingleFileAsync();

        if (file != null)
        {
           
            BitmapImage avatarImage = new BitmapImage(new Uri(file.Path));
            ViewModel.informations.AvatarUrl = avatarImage;
        }
        else
        {
            //TODO
        }
    }

    private void RemoveAvatar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.informations.SetAvatarUrl("ms-appx:///Assets/defaultAvt.png");
    }
}
