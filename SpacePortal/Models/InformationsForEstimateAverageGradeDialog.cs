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

    private Dictionary<string, double> _degreeTypesWithTheirGrade;

    public Dictionary<string, double> DegreeTypesWithTheirGrade
    {
        get
        {
            var sortedList = _degreeTypesWithTheirGrade
                .OrderByDescending(kvp => kvp.Value);
            return sortedList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        set => _degreeTypesWithTheirGrade = value;
    }
}
