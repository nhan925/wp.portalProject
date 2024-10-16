using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;

public class InformationsForDashboard : INotifyPropertyChanged
{


    public string StudentFullName
    {
        get; set;
    }

    public string StudentName => StudentFullName.Split(' ')[^1];

    public ObservableCollection<KeyValuePair<string, double>> GPAs
    {
        get; set;
    }

    public double CGPA
    {
        get; set;
    }

    public int CurrentCredit
    {
        get; set;
    }

    public int TotalCredit
    {
        get; set;
    }

    public ObservableCollection<KeyValuePair<string, int>> CreditDistribution
    {
        get
        {
            var resourceLoader = new Microsoft.Windows.ApplicationModel.Resources.ResourceLoader();
            return new ObservableCollection<KeyValuePair<string, int>>()
            {
              new (resourceLoader.GetString("Dashboard_CreditsChartCurrent"), CurrentCredit),
              new (resourceLoader.GetString("Dashboard_CreditsChartRemain"), TotalCredit - CurrentCredit),
            };
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
