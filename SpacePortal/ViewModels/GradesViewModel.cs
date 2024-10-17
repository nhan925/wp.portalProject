using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.Helpers;

namespace SpacePortal.ViewModels;

public partial class GradesViewModel : ObservableRecipient
{
    //------------DataGrid Grades Table--------------
    private IDao<GradesRow> _dao;
    public ObservableCollection<GradesRow> Grades {get; set;}
    public ObservableCollection<GradesRow> SourceData {get; set;}

    //------------ComboBox Year and Semester--------------
    public ObservableCollection<string> Years { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Semesters { get; set; } = new ObservableCollection<string>();

    //------------TextBlock General Information--------------
    public double GpaScale_4 {get; set;}
    public double GpaScale_10 {get; set;}
    public int NumberOfCourses {get; set;}
    public int NumberOfCredits { get; set; }


    public GradesViewModel()
    {
        _dao = new InformationsForGradesPageDao();
        Grades = new ObservableCollection<GradesRow>();
        SourceData = new ObservableCollection<GradesRow>(_dao.GetAll());
    }

    public void Init()
    {
        //-----------ComboBox Year and Semester---------------
        AddYears(2022);
        AddSemester();

        //-----------TextBlock General Information------------
        NumberOfCourses = SourceData.Count;
        for (int i = 0; i < SourceData.Count; i++)
        {
            GpaScale_4 += SourceData[i].GradeScaleFour;
            GpaScale_10 += SourceData[i].GradeScaleTen;
            NumberOfCredits += SourceData[i].CourseCredit;
        }
        GpaScale_4 = Math.Round(GpaScale_4 / SourceData.Count , 2);
        GpaScale_10 = Math.Round(GpaScale_10 / SourceData.Count, 2);

        //-----------DataGrid Grades Table-------------
        if (Grades.Any()) { Grades.Clear();}
        foreach (var grade in SourceData)
        {
            Grades.Add(grade);
        }
    }


    //----------------Generral Information
    public void AddYears(int startYear)
    {
        var resourceLoader = new ResourceLoader();
        var defaultOption = resourceLoader.GetString("GradesPage_ComboBoxDefaultOption");
        Years.Add(defaultOption);

        // Add a list of 5 academic years for university
        for (int i = 0; i < 4; i++)
        {
            int endYear = startYear + 1;
            Years.Add($"{startYear} - {endYear}");
            startYear++;
        }
    }

    public void AddSemester()
    {
        var resourceLoader = new ResourceLoader();
        var defaultOption = resourceLoader.GetString("GradesPage_ComboBoxDefaultOption");
        Semesters.Add(defaultOption);
        Semesters.Add("1");
        Semesters.Add("2");
        Semesters.Add("3");
    }


    //-----------------Grades DataGrid
    public void ShowGradeByYear(string year)
    {
        var gradesByYear = ((InformationsForGradesPageDao)_dao).GetByYear(SourceData, year);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesByYear.Any())
        {
            foreach (var grade in gradesByYear)
            {
                Grades.Add(grade);
            }
        }
    }

    public void ShowGradeBySemester(string semester)
    {
        var gradesBySemester = ((InformationsForGradesPageDao)_dao).GetBySemester(SourceData, semester);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesBySemester.Any())
        {
            foreach (var grade in gradesBySemester)
            {
                Grades.Add(grade);
            }
        }
    }

    public void ShowGradeByYearAndSemester(string year, string semester)
    {
        var gradesByYear = ((InformationsForGradesPageDao)_dao).GetByYear(SourceData, year);
        var gradesBySemester = ((InformationsForGradesPageDao)_dao).GetBySemester(gradesByYear, semester);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesBySemester.Any())
        {
            foreach (var grade in gradesBySemester)
            {
                Grades.Add(grade);
            }
        }
    }
}
