using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;
public class RequestPhysicalTranscriptDialogViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public RequestPhysicalTranscriptDialogViewModel(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData)
    {
        Years = new();
        CombinedSemesterAndYear = new();

        var sourceDataList = sourceData.ToList();
        sourceDataList.Sort((x, y) => ($"{x.Year}/{x.Semester}").CompareTo($"{y.Year}/{y.Semester}"));

        foreach (var row in sourceDataList)
        {
            if (!Years.Contains(row.Year))
            {
                Years.Add(row.Year);
            }

            if (!CombinedSemesterAndYear.Contains($"{row.Year}/{row.Semester}"))
            {
                CombinedSemesterAndYear.Add($"{row.Year}/{row.Semester}");
            }
        }

        NumberOfTranscriptOfAllVi = 0;
        NumberOfTranscriptOfAllEn = 0;
        NumberOfSemeseterTranscriptsVi = 0;
        NumberOfSemeseterTranscriptsEn = 0;
        NumberOfYearTranscriptsVi = 0;
        NumberOfYearTranscriptsEn = 0;
    }

    public ObservableCollection<string> Years
    {
        get; set;
    }

    public ObservableCollection<string> CombinedSemesterAndYear
    {
        get; set;
    }

    public int TotalTranscripts => NumberOfTranscriptOfAllVi + NumberOfTranscriptOfAllEn +
                                   NumberOfSemeseterTranscriptsVi + NumberOfSemeseterTranscriptsEn +
                                   NumberOfYearTranscriptsVi + NumberOfYearTranscriptsEn;

    public int NumberOfTranscriptOfAllVi
    {
        get; set;
    }

    public int NumberOfTranscriptOfAllEn
    {
        get; set;
    }

    public int NumberOfSemeseterTranscriptsVi
    {
        get; set;
    }

    public int NumberOfSemeseterTranscriptsEn
    {
        get; set;
    }

    public int NumberOfYearTranscriptsVi
    {
        get; set;
    }

    public int NumberOfYearTranscriptsEn
    {
        get; set;
    }

    public void SendRequestForTranscripts()
    {
        // TODO: Summary information & send request for transcripts
        // string requestContent = information of student and transcripts (type, language, quantity)
    }
}
