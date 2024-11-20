using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;

public class ChooseClassesInformations : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string CourseId
    {
        get; set;
    }

    public string CourseName
    {
        get; set;
    }

    public string Status
    {
        get; set;
    }

    public List<ClassOfCourse> Classes
    {
        get; set;
    }

    public int NumberOfClasses => Classes.Count;

    public string RegisteredClassId
    {
        get; set;
    }
}
