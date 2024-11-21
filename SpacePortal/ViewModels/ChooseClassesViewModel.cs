using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
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
        Informations.PeriodId = periodId;
        Informations.CourseId = courseId;
        Informations.CourseName = courseName;
        Informations.Status = status;
    }

    public string CancelCourse(string registeredClassId)
    {
        var result = App.GetService<ApiService>().Post<string>("/rpc/cancel_course", new { canceled_class_id = registeredClassId });
        return result;
    }

    public string RegisterClass(string classId, string registeredClassId)
    {
        var result = App.GetService<ApiService>().Post<string>("/rpc/register_class", new { to_register_class_id = classId, registered_class_id = registeredClassId });
        return result;
    }
}
