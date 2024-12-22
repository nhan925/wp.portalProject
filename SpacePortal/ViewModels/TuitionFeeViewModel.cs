using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class TuitionFeeViewModel : ObservableRecipient
{
    public ObservableCollection<TuitionFeeListInformations> Information
    {
        get; set;
    }

    public TuitionFeeViewModel()
    {
        Init();
    }

    public void Init()
    {
        Information = App.GetService<IDao<TuitionFeeListInformations>>().GetAll();
    }
}
