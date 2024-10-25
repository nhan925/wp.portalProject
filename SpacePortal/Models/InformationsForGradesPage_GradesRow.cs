using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class InformationsForGradesPage_GradesRow : INotifyPropertyChanged
{
    public string Year
    {
        get; set;
    }

    public string Semester
    {
        get; set;
    }

    public string CourseId
    {
        get; set;
    }

    public string CourseName
    {
        get; set;
    }

    public int CourseCredit
    {
        get; set;
    }

    public string ClassId
    {
        get; set;
    }

    public double GradeScaleTen
    {
        get; set;
    }

    public double GradeScaleFour
    {
        get; set;
    }

    public string Note
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
