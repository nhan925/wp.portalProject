using System.Collections.ObjectModel;
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

    public ObservableCollection<Course> UnregisteresCoursesSearch { get; set; }

    private string _periodId;

    public void LoadInformations(int periodId, string title)
    {
        _periodId = periodId.ToString();
        Informations = App.GetService<IDao<ChooseCoursesInformations>>().GetById(_periodId);
        Informations.Title = title.ToUpper();
        UnregisteresCoursesSearch = new ObservableCollection<Course>(Informations.UnregisteredCourses);
    }

    public void SearchUnregisteredCourses(string search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            UnregisteresCoursesSearch = new ObservableCollection<Course>(Informations.UnregisteredCourses
                .Where(c => c.Name.ToLower().Contains(search.ToLower()) || c.Id.ToLower().Contains(search.ToLower())));
        }
        else
        {
            UnregisteresCoursesSearch = new ObservableCollection<Course>(Informations.UnregisteredCourses);
        }
    }
}
