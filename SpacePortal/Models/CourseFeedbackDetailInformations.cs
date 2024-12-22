using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class CourseFeedbackDetailInformations : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string courseName
    {
        get;
        set;
    }

    public string courseID
    {
        get; set;
    }

    public string teacherName
    {
        get; set;
    }

    public string classID
    {
        get; set;
    }
}
