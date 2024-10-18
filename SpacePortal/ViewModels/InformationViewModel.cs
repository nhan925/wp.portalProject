using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Models;
using SpacePortal.Models;
using SpacePortal.Core.Contracts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Globalization;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System.Net;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Text.RegularExpressions;
namespace SpacePortal.ViewModels;

public partial class InformationViewModel : ObservableRecipient
{
    
    public InformationsForInformation informations
    {
        get; set;
    }

    [ObservableProperty]
    private string _editPersonalEmail;

    [ObservableProperty]
    private string _editPhoneNumber;

    [ObservableProperty]
    private string _editAddress;

    [ObservableProperty]
    private bool _isEditing;

    public InformationViewModel()
    {
        informations = App.GetService<IDao<InformationsForInformation>>().GetById(1);

        EditPersonalEmail = informations.PersonalEmail;
        EditPhoneNumber = informations.PhoneNumber;
        EditAddress = informations.Address;
        IsEditing = false;
    }

    public void AcceptChanges()
    {   
        informations.PersonalEmail = EditPersonalEmail;
        informations.PhoneNumber = EditPhoneNumber;
        informations.Address = EditAddress;
        IsEditing = false;
    }

    public string CheckAcceptChanges()
    {
        ResourceLoader resourceLoader = new ResourceLoader();

        if (string.IsNullOrWhiteSpace(EditPersonalEmail) || string.IsNullOrWhiteSpace(EditPhoneNumber) || string.IsNullOrWhiteSpace(EditAddress))
        {
            return resourceLoader.GetString("Information_Error_NotEmpty/Text");
        }

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|org|net|edu)$";
        if (!Regex.IsMatch(EditPersonalEmail, emailPattern))
        {
            return resourceLoader.GetString("Information_Error_Email/Text");
        }

        if (EditPhoneNumber.Length != 10 || !EditPhoneNumber.All(char.IsDigit))
        {
            return resourceLoader.GetString("Information_Error_PhoneNumber/Text");
        }

        return "Success";
    }
        
    public void CancelChanges()
    {
        EditPersonalEmail = informations.PersonalEmail;
        EditPhoneNumber = informations.PhoneNumber;
        EditAddress = informations.Address;
        IsEditing = false;
    }

    public void ChangeContact()
    {
        if (IsEditing)
        {
            EditPersonalEmail = informations.PersonalEmail;
            EditPhoneNumber = informations.PhoneNumber;
            EditAddress = informations.Address;
        }
        IsEditing = !IsEditing;
    }

    public async void UploadAvatar()
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
            informations.AvatarUrl = avatarImage;
            //Update database
        }
        else
        {
            //TODO
        }
    }

    public void RemoveAvatar()
    {
        informations.SetAvatarUrl("ms-appx:///Assets/defaultAvt.png");
    }
}