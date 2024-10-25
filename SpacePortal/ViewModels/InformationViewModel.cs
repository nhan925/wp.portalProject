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
using SpacePortal.Core.Services;
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

        if (string.IsNullOrEmpty(informations.AvatarUrl))
        {
            informations.SetAvatarBitmap("ms-appx:///Assets/defaultAvt.png");
        }
        else
        {
            informations.SetAvatarBitmap(informations.AvatarUrl);
        }


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

    public void Save()
    {
        var personal_email_update = EditPersonalEmail;
        var phone_number_update = EditPhoneNumber;
        var address_update = EditAddress;
        var contactInfo = new
        {
            personal_email_update,
            phone_number_update,
            address_update
        };

        string result = App.GetService<ApiService>().Post<string>("/rpc/update_contact_information", contactInfo);


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

            informations.SetAvatarBitmap(file.Path);
            UploadAvatarToDatabase(file.Path);
        }
        else
        {
            //TODO
        }
    }

    public void RemoveAvatar()
    {
        informations.SetAvatarBitmap("ms-appx:///Assets/defaultAvt.png");
        string result = App.GetService<ApiService>().Post<string>("/rpc/update_avatar", new { image_url = "" });
    }

    private async Task UploadAvatarToDatabase (string filePath)
    {
        var imgurService = App.GetService<ImgurService>();
        string imageUrl = await imgurService.UploadImageAsync(filePath);

        if (!string.IsNullOrEmpty(imageUrl))
        {
            string result = App.GetService<ApiService>().Post<string>("/rpc/update_avatar", new { image_url = imageUrl });
        }
        else
        {
            Console.WriteLine("Image upload failed.");
        }
    }
}