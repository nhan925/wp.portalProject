using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;

public class ClassOfCourse: INotifyPropertyChanged
{
    public string Id
    {
        get; set;
    }

    public string Name
    {
        get; set; 
    }

    public string Day
    {
        get; set;
    }

    public string Time
    {
        get; set;
    }

    public string Room
    {
        get; set;
    }

    public int MaxSlot 
    { 
        get; set; 
    }

    public int RegisteredSlot
    {
        get; set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
