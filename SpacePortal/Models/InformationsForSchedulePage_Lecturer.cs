using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class InformationsForSchedulePage_Lecturer : INotifyPropertyChanged
{
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
    public string AcademicRank
    {
        get; set;
    }
    public string AcademicDegree
    {
        get; set;
    }
    public string FacultyName
    {
        get; set;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
