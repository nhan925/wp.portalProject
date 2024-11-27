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
           var lines = Content.Split("\r\n");
            return lines[0];
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
