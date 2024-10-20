using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class InformationsForEstimateAverageGradeDialog : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int TotalCredits
    {
        get; set;
    }

    public Dictionary<string, double> DegreeTypesWithTheirGrade
    {
        get; set;
    }
}
