using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Models;
using SpacePortal.Models;
using SpacePortal.Core.Contracts;
namespace SpacePortal.ViewModels;

public partial class InformationViewModel : ObservableRecipient
{
    public InformationsForInformation informations
    {
        get; set;
    }

    public InformationViewModel()
    {
        informations = App.GetService<IDao<InformationsForInformation>>().GetById(1);
    }

   
}