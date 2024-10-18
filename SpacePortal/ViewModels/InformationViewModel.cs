using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Models;
using SpacePortal.Models;
using SpacePortal.Core.Contracts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Globalization;
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
    

    public void CancelChanges()
    {
        EditPersonalEmail = informations.PersonalEmail;
        EditPhoneNumber = informations.PhoneNumber;
        EditAddress = informations.Address;
        IsEditing = false;
    }


}