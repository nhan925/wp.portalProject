using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class InformationsForRequestDetailPage_Answer : INotifyPropertyChanged
{
    public int AnswerId 
    { 
        get; set; 
    }

    public string AdminName
    {
        get; set;
    }

    public string AnswerContent
    {
        get; set;
    }

    public DateTime SubmittedAt
    {
        get; set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
