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
        get
        {
            if (GradeScaleTen >= 9.0)
                return 4.0;
            else if (GradeScaleTen >= 8.0)
                return 3.5;
            else if (GradeScaleTen >= 7.0)
                return 3.0;
            else if (GradeScaleTen >= 6.0)
                return 2.5;
            else if (GradeScaleTen >= 5.0)
                return 2.0;
            else if (GradeScaleTen >= 4.0)
                return 1.0;
            else
                return 0.0;
        }
    }

    public string Note
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;

}
