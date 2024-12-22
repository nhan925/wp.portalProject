using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Models;
using Syncfusion.UI.Xaml.Scheduler;
using Windows.Data.Xml.Dom;


namespace SpacePortal.ViewModels;

public partial class ScheduleViewModel : ObservableRecipient
{
    private IDao<InformationsForSchedulePage_Class> _dao;

    [ObservableProperty]
    public DateTime _minimumDate;

    [ObservableProperty]
    public DateTime _maximumDate;

    public ObservableCollection<InformationsForSchedulePage_Class> Schedule
    {
        get; set;
    }

    public ObservableCollection<string> Semesters { get; set; } = new ObservableCollection<string>();

    public InformationsForSchedulePageDao DaoForComboBox
    {
        get; set;
    }

    public ScheduleViewModel()
    {
        _dao = App.GetService<IDao<InformationsForSchedulePage_Class>>();
        InitializeSchedule();
    }

    void InitializeSchedule()
    {
        DaoForComboBox = (_dao as InformationsForSchedulePageDao);
        Semesters = DaoForComboBox?.GetSemesters();
        var DateOfMonday = GetDateOfDayOfWeek(DayOfWeek.Monday);
        var DateOfSunday = GetDateOfDayOfWeek(DayOfWeek.Sunday);
        MinimumDate = new DateTime(DateOfMonday.Year, DateOfMonday.Month, DateOfMonday.Day,1,0,0);
        MaximumDate = new DateTime(DateOfSunday.Year, DateOfSunday.Month, DateOfSunday.Day, 10, 59, 59);
    }

    public void ShowScheduleBySemester(string Semester)
    {
        var parts = Semester.Split('/');
        var keywords = new List<string> { parts[0], parts[1] };
        Schedule = _dao.GetAll(null, null, keywords);
    }

    private DateTime GetDateOfDayOfWeek(DayOfWeek targetDay)
    {
        var convertedTargetDay = targetDay == DayOfWeek.Sunday ? 6 : (int)targetDay - 1;
        var convertedToday = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)DateTime.Now.DayOfWeek - 1;
        int daysToTarget = (convertedTargetDay - convertedToday);

        return DateTime.Now.AddDays(daysToTarget);
    }
}
