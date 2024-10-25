using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SpacePortal.Models;

public class InformationsForGradesPage_GeneralInformation : INotifyPropertyChanged
{
    public double GpaScale_4
    {
        get;set;
    }

    public double GpaScale_10
    {
        get; set;
    }

    public int NumberOfCourses
    {
        get; set;
    }

    public int NumberOfCredits
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}



