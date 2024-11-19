using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class ChooseCoursesInformations : INotifyPropertyChanged
{
    public class Course
    {
        public string Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int Credits
        {
            get; set;
        }

        public string ClassName
        {
            get; set;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Title
    {
        get; set;
    }

    public int MaxCredits
    {
        get; set;
    }

    public string Major
    {
        get; set;
    }

    public int RegisteredCoursesCount
    {
        get; set;
    }

    public int RegisteredCredits
    {
        get; set;
    }

    public List<Course> UnregisteredCourses
    {
        get; set;
    }

    public List<Course> RegisteredCourses
    {
        get; set;
    }

    public List<Course> StudiedCourses
    {
        get; set;
    }
}
