using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Models;
public class InformationsForSchedulePage_Lecturer : INotifyPropertyChanged
{
    private ResourceLoader resourceLoader = new();

    public string LecturerId
    {
        get; set;
    }
    public string FullName
    {
        get; set;
    }
    public string Gender
    {
        get; set;
    }
    public string PhoneNumber
    {
        get; set;
    }
    public string Email
    {
        get; set;
    }

    private string _academicRank;
    public string AcademicRank
    {
        get
        {
            if (String.IsNullOrEmpty(_academicRank))
            {
                return resourceLoader.GetString("Schedule_AcademicRankAndDegreeNone");
            }
            else
            {
                return _academicRank;
            }
        }

        set => _academicRank = value;
    }

    private string _academicDegree;
    public string AcademicDegree
    {
        get
        {
            if (String.IsNullOrEmpty(_academicDegree))
            {
                return resourceLoader.GetString("Schedule_AcademicRankAndDegreeNone");
            }
            else
            {
                return _academicDegree;
            }
        }

        set => _academicDegree = value;
    }
    public string FacultyName
    {
        get; set;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
