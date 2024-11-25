using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Models;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Services;

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

    public void SendRequestForTranscripts(string selectedSemester = "", string selectedYear = "")
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        StringBuilder requestContent = new StringBuilder();
        var language_en = resourceLoader.GetString("GradesPage_RequestForTranscriptEn/Text");
        var language_vi = resourceLoader.GetString("GradesPage_RequestForTranscriptVi/Text");
        var processingStatus = resourceLoader.GetString("RequestPage_ProcessingStatus/Text");

        requestContent.AppendLine(resourceLoader.GetString("GradesPage_RequestForTranscriptTitle"));
        if (NumberOfTranscriptOfAllVi > 0 || NumberOfTranscriptOfAllEn > 0)
        {
            requestContent.AppendLine($"{resourceLoader.GetString("GradesPage_RequestForTranscriptAll/Text")},{language_vi}: {NumberOfTranscriptOfAllVi},{language_en}: {NumberOfTranscriptOfAllEn} ");
        }
        if (NumberOfSemeseterTranscriptsVi > 0 || NumberOfSemeseterTranscriptsEn > 0)
        {
            requestContent.AppendLine($"{resourceLoader.GetString("GradesPage_RequestForTranscriptSemesterOfYear/Text")}: {selectedSemester},{language_vi}: {NumberOfSemeseterTranscriptsVi},{language_en}: {NumberOfSemeseterTranscriptsEn}");
        }
        if (NumberOfYearTranscriptsVi > 0 || NumberOfYearTranscriptsEn > 0)
        {
            requestContent.AppendLine($"{resourceLoader.GetString("GradesPage_RequestForTranscriptYear/Text")}: {selectedYear},{language_vi}: {NumberOfYearTranscriptsVi},{language_en}: {NumberOfYearTranscriptsEn}");
        }
        requestContent.Append($"{resourceLoader.GetString("GradesPage_RequestForTranscriptTotal/Text")}: {TotalTranscripts}");
        
        // Send request
        var result = App.GetService<ApiService>().Post<string>("/rpc/add_request", new { content = requestContent.ToString(), status = processingStatus });
    }
}
