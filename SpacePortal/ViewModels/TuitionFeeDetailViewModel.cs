using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class TuitionFeeDetailViewModel : ObservableRecipient
{

    public TuitionFeeDetailInformations Informations
    {
        get; set;
    }

    public ObservableCollection<TuitionFeeDetailCourse> courseDetail
    {
        get; set;
    }


    public void LoadInformations(string year, int semester, int totalCourse, int totalTuitionFee)
    {

        courseDetail = (App.GetService<IDao<TuitionFeeDetailCourse>>() as TuitionFeeDetailCourseDao).GetCourseList(year, semester);
        Informations = new TuitionFeeDetailInformations();
        Informations.year = year;
        Informations.semester = semester;
        Informations.totalCourse = totalCourse;
        Informations.totalTuitionFee = totalTuitionFee;
    }

}
