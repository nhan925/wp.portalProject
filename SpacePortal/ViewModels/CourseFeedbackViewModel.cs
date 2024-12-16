using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class CourseFeedbackViewModel : ObservableRecipient
{
    public ObservableCollection<CourseFeedbackListInformations> Information
    {
        get; set;
    }

    public CourseFeedbackViewModel()
    {
        Init();
    }

    public void Init()
    {
        Information = App.GetService<IDao<CourseFeedbackListInformations>>().GetAll();
    }
}
