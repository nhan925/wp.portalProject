using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class ChooseCoursesViewModel : ObservableRecipient
{
    public ChooseCoursesInformations Informations
    {
        get; set;
    }

    private string _periodId;

    public void LoadInformations(int periodId, string title)
    {
        _periodId = periodId.ToString();
        Informations = App.GetService<IDao<ChooseCoursesInformations>>().GetById(_periodId);
        Informations.Title = title.ToUpper();
    }

    public void RefreshInformations()
    {
        Informations = App.GetService<IDao<ChooseCoursesInformations>>().GetById(_periodId);
    }
}
