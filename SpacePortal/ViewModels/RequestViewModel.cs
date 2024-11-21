using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    public ObservableCollection<InformationsForRequest_RequestRow> Requests { get; set; }
 

    public RequestViewModel()
    {
        Requests = App.GetService<IDao<InformationsForRequest_RequestRow>>().GetAll();
    }
}
