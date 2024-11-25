using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Models;
public class InformationsForRequest_RequestRow : INotifyPropertyChanged
{
    public int TotalRequests
    {
        get; set;
    }
    public int SequenceNumber
    {
        get; set;
    }
    public int RequestId
    {
        get; set;
    }

    public string StudentId
    {
        get; set;
    }

    public string StudentName
    {
        get; set;
    }

    public string RequestName
    {
        get
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            var requestReExamiantion = resourceLoader.GetString("GradesPage_RequestForReviewTitle");
            var requestPhysicalTranscript = resourceLoader.GetString("GradesPage_RequestForTranscriptTitle");

            var requestName = "";
                
            var lines = Content.Split("\r\n");
            if (lines[0] == requestReExamiantion)
            {
                var semesterString = resourceLoader.GetString("GradesPage_RequestForReviewSemester/Text");
                requestName = $"{lines[0]} {semesterString} {lines[2].Split(": ")[1]} - {lines[1].Replace($": ", $" ").Trim()}";
            }
            else if (lines[0] == requestPhysicalTranscript)
            {
                var totalTranscriptString = resourceLoader.GetString("GradesPage_RequestForTranscriptTotal/Text");
                var numberOfTranscript = lines[lines.Length - 1].Split(": ")[1];
                requestName = $"{lines[0]} - {totalTranscriptString}: {numberOfTranscript}";
            }
            return requestName;
        }
    }

    public DateTime SubmittedAt
    {
        get; set;
    }

    public string Content
    {
        get; set;
    }

    public string Status
    {
        get; set;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
