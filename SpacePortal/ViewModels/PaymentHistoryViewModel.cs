using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class PaymentHistoryViewModel : ObservableRecipient
{
    public ObservableCollection<PaymentHistoryInfomations> Informations
    {
        get; set;
    }
    
    public PaymentHistoryViewModel()
    {
        Informations = App.GetService<IDao<PaymentHistoryInfomations>>().GetAll();
    }
}
