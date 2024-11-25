using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Helpers;
using SpacePortal.Models;
using Sprache;

namespace SpacePortal.ViewModels;

public partial class ChooseCoursesViewModel : ObservableRecipient
{
    public ChooseCoursesInformations Informations
    {
        get; set;
    }

    public ObservableCollection<Course> UnregisteresCoursesSearch { get; set; }

    public string PeriodId
    {
        get; set;
    }

    public void LoadInformations(int periodId, string title)
    {
        PeriodId = periodId.ToString();
        Informations = App.GetService<IDao<ChooseCoursesInformations>>().GetById(PeriodId);
        Informations.Title = title.ToUpper();
        UnregisteresCoursesSearch = new ObservableCollection<Course>(Informations.UnregisteredCourses);
    }

    // Full-text search
    public void SearchUnregisteredCourses(string search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = Regex.Replace(search.Trim(), @"\s+", " ").NormalizeSearch();
            UnregisteresCoursesSearch = new ObservableCollection<Course>(
                Informations.UnregisteredCourses.Where(c =>
                    c.Name.NormalizeSearch().Contains(normalizedSearch) ||
                    c.Id.NormalizeSearch().Contains(normalizedSearch)));
        }
        else
        {
            UnregisteresCoursesSearch = new ObservableCollection<Course>(Informations.UnregisteredCourses);
        }
    }
}
