using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class ChooseClassesViewModel : ObservableRecipient
{
    public ChooseClassesInformations Informations
    {
        get; set;
    }

    public void LoadInformations(string periodId, string courseId, string courseName, string status)
    {
        Informations = (App.GetService<IDao<ChooseClassesInformations>>() as ChooseClassesInformationsDao).GetById(courseId, periodId);
        Informations.CourseId = courseId;
        Informations.CourseName = courseName;
        Informations.Status = status;
    }
}
