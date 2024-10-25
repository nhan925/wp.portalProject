using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SpacePortal.Models;

public class InformationsForGradesPage_GeneralInformation : INotifyPropertyChanged
{
    private double gpaScale_4;
    private double gpaScale_10;
    private int numberOfCourses;
    private int numberOfCredits;

    public double GpaScale_4
    {
        get
        {
            return gpaScale_4;
        }
        set
        {
            if (gpaScale_4 != value)
            {
                gpaScale_4 = value;
                OnPropertyChanged(nameof(GpaScale_4));
            }
        }
    }

    public double GpaScale_10
    {
        get
        {
            return gpaScale_10;
        }
        set
        {
            if (gpaScale_10 != value)
            {
                gpaScale_10 = value;
                OnPropertyChanged(nameof(GpaScale_10));
            }
        }
    }

    public int NumberOfCourses
    {
        get
        {
            return numberOfCourses;
        }
        set
        {
            if (numberOfCourses != value)
            {
                numberOfCourses = value;
                OnPropertyChanged(nameof(NumberOfCourses));
            }
        }
    }

    public int NumberOfCredits
    {
        get
        {
            return numberOfCredits;
        }
        set
        {
            if (numberOfCredits != value)
            {
                numberOfCredits = value;
                OnPropertyChanged(nameof(NumberOfCredits));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}



