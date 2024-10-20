using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Models;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;
public class EstimateAverageGradeDialogViewModel: INotifyPropertyChanged
{
    public EstimateAverageGradeDialogViewModel(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData,
        InformationsForEstimateAverageGradeDialog degreeAndCreditInfo)
    {
        DegreeTypes = new ObservableCollection<string>();
        foreach (var item in degreeAndCreditInfo.DegreeTypesWithTheirGrade)
        {
            DegreeTypes.Add(item.Key);
        }

        var currentCredits = 0;
        var productSum = 0.0;
        foreach (var item in sourceData)
        {
            currentCredits += item.CourseCredit;
            productSum += Convert.ToDouble(item.CourseCredit) * item.GradeScaleTen;
        }

        RemainCredits = degreeAndCreditInfo.TotalCredits - currentCredits;

        EstimateGradeForEachDegreeType = new();
        foreach (var item in DegreeTypes)
        {
            EstimateGradeForEachDegreeType[item] = 
                (degreeAndCreditInfo.TotalCredits * degreeAndCreditInfo.DegreeTypesWithTheirGrade[item] - productSum) / RemainCredits;
        }

        EstimatedGradeString = "-";
    }

    public ObservableCollection<string> DegreeTypes
    {
        get;
    }

    public int RemainCredits
    {
        get;
    }

    public string EstimatedGradeString
    {
        get;
        set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void CalculateEstimatedGrade(string degreeType)
    {

        var currentLanguage = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        CultureInfo culture = new CultureInfo(currentLanguage);
        var estimatedGrade = EstimateGradeForEachDegreeType[degreeType];

        if (estimatedGrade < 0)
        {
            EstimatedGradeString = (new ResourceLoader()).GetString("GradesPage_EstimateAverageGradeAbsolutely");
        }
        else if (estimatedGrade > 10)
        {
            EstimatedGradeString = (new ResourceLoader()).GetString("GradesPage_EstimateAverageGradeImpossible");
        }
        else
        {
            EstimatedGradeString = EstimateGradeForEachDegreeType[degreeType].ToString("F2", culture);
        }
    }

    private Dictionary<string, double> EstimateGradeForEachDegreeType
    {
        get; set;
    }
}
