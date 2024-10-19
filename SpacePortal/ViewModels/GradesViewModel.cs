using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.Helpers;
using SpacePortal.Models;
using SpacePortal.Models.SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class GradesViewModel : ObservableRecipient
{
    //------------DataGrid Grades Table--------------
    private IDao<InformationsForGradesPage_GradesRow> _dao;
    public ObservableCollection<InformationsForGradesPage_GradesRow> Grades {get; set;}
    public ObservableCollection<InformationsForGradesPage_GradesRow> SourceData {get; set;}

    //------------ComboBox Year and Semester--------------
    public ObservableCollection<string> Years { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Semesters { get; set; } = new ObservableCollection<string>();
    public string DefaultOption {get;set;}

    //------------TextBlock General Information--------------
    public InformationsForGradesPage_GeneralInformation GeneralInformations { get;set; } = new InformationsForGradesPage_GeneralInformation();


    public GradesViewModel()
    {
        _dao = App.GetService<IDao<InformationsForGradesPage_GradesRow>>();
        SourceData = new ObservableCollection<InformationsForGradesPage_GradesRow>(_dao.GetAll());
        Grades = new ObservableCollection<InformationsForGradesPage_GradesRow>(SourceData);
        DefaultOption = new ResourceLoader().GetString("GradesPage_ComboBoxDefaultOption");
        AddYears(2022);
        AddSemester();
    }

    public void Init()
    {
        UpdateGeneralInformation(SourceData);
        if (Grades.Any()) { Grades.Clear();}
        foreach (var grade in SourceData)
        {
            Grades.Add(grade);
        }
    }


    //----------------Generral Information
    public void AddYears(int startYear)
    {
        Years.Add(DefaultOption);
        for (int i=0;i<SourceData.Count;i++) {
            if (!Years.Contains(SourceData[i].Year))
            {
                Years.Add(SourceData[i].Year);
            }
        }
    }

    public void AddSemester()
    {
        Semesters.Add(DefaultOption);
        Semesters.Add("1");
        Semesters.Add("2");
        Semesters.Add("3");
    }

    public void UpdateGeneralInformation(ObservableCollection<InformationsForGradesPage_GradesRow> grades)
    {
        //-----------TextBlock General Information------------
        if(grades.Count == 0)
        {
            GeneralInformations.GpaScale_4 = 0;
            GeneralInformations.GpaScale_10 = 0;
            GeneralInformations.NumberOfCourses = 0;
            GeneralInformations.NumberOfCredits = 0;
            return;
        }
        double gpaScale_4 = 0;
        double gpaScale_10 = 0;
        int numberOfCredits = 0;
        for (int i = 0; i < grades.Count; i++)
        {
            gpaScale_4 += grades[i].GradeScaleFour;
            gpaScale_10 += grades[i].GradeScaleTen;
            numberOfCredits += grades[i].CourseCredit;
        }
        GeneralInformations.NumberOfCourses = grades.Count;
        GeneralInformations.GpaScale_4 = Math.Round(gpaScale_4 / grades.Count, 2);
        GeneralInformations.GpaScale_10 = Math.Round(gpaScale_10 / grades.Count, 2);
        GeneralInformations.NumberOfCredits = numberOfCredits;
    }

    //-----------------Grades DataGrid------------------------
    public void ShowGradeByYear(string year)
    {
        var gradesByYear = GetByYear(SourceData, year);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesByYear.Any())
        {
            foreach (var grade in gradesByYear)
            {
                Grades.Add(grade);
            }
        }
        UpdateGeneralInformation(Grades);
    }

    public void ShowGradeByYearAndSemester(string year, string semester)
    {
        var gradesByYear = GetByYear(SourceData, year);
        var gradesBySemester = GetBySemester(gradesByYear, semester);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesBySemester.Any())
        {
            foreach (var grade in gradesBySemester)
            {
                Grades.Add(grade);
            }
        }
        UpdateGeneralInformation(Grades);
    }

    public ObservableCollection<InformationsForGradesPage_GradesRow> GetByYear(ObservableCollection<InformationsForGradesPage_GradesRow> rows, string years)
    {
        if (years == DefaultOption)
        {
            return rows;
        }
        var result = new ObservableCollection<InformationsForGradesPage_GradesRow>();
        for (var i = 0; i < rows.Count; i++)
        {
            if (rows[i].Year == years)
            {
                result.Add(rows[i]);
            }
        }
        return result;
    }

    public ObservableCollection<InformationsForGradesPage_GradesRow> GetBySemester(ObservableCollection<InformationsForGradesPage_GradesRow> rows, string semester)
    {
        if (semester == DefaultOption)
        {
            return rows;
        }
        var result = new ObservableCollection<InformationsForGradesPage_GradesRow>();
        for (var i = 0; i < rows.Count; i++)
        {
            if (rows[i].Semester == semester)
            {
                result.Add(rows[i]);
            }
        }
        return result;
    }


}
