using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class CoursesRegistrationViewModel : ObservableRecipient
{
    public ObservableCollection<CoursesRegistrationPeriodInformation> Information
    {
        get;
    }

    public CoursesRegistrationViewModel()
    {
        Information = App.GetService<IDao<CoursesRegistrationPeriodInformation>>().GetAll();
    }
}
