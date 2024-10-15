using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;

public class InformationsForDashboard: INotifyPropertyChanged
{
    public string StudentFullName
    {
        get; set;
    }

    public string StudentName => StudentFullName.Split(' ')[^1];

    public Dictionary<string, double> GPAs
    {
        get; set;
    }

    public double CGPA
    {
        get; set;
    }

    public int TotalCredits
    {
        get; set;
    }

    public int CurrentCredits
    {
        get; set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
