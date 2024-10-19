using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Core.Models;
public class GradesRow : INotifyPropertyChanged
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
    public string ClassID
    {
        get; set;
    }
    public int GradeScaleTen
    {
        get; set;
    }
    public int GradeScaleFour
    {
        get; set;
    }
    public string Note
    {
        get; set;
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
