using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Models;
public class InformationsForScholarshipPage
{
    private readonly ResourceLoader resourceLoader = new();
    public int Id
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public double Amount
    {
        get; set;
    }
    public string Currency
    {
        get; set;
    }
    public int Slot
    {
        get; set;
    }
    public string Sponsor
    {
        get; set;
    }
    public string Criteria
    {
        get; set;
    }
    public DateTime AnouncementDate
    {
        get; set;
    }
    public DateTime Deadline
    {
        get; set;
    }
    public bool Applied
    {
        get; set;
    }
    public bool Expired
    {
        get; set;
    }
    public string ApplicationStatus { get; set; } = String.Empty;
    public DateTime? SubmittedDate { get; set; } = new();

    private string _attachment = String.Empty;
    public string Attachment
    {
        get
        {
            if (String.IsNullOrEmpty(_attachment))
            {
                return "https://google.com";
            }
            else
            {
                return _attachment;
            }
        }
        set => _attachment = value;
    }

    public List<string> RequiredDocuments { get; set; } = new();

    public string RequiredDocumentsString => string.Join("\n", RequiredDocuments.Select(doc => $"- {doc}"));

    public KeyValuePair<String, String> StatusWithColor
    {
        get
        {
            if (Expired)
            {
                return new(resourceLoader.GetString("Scholarship_Expired"), "Red");
            }

            if (Applied)
            {
                return new (resourceLoader.GetString("Scholarship_Applied"), "LimeGreen");
            }
            else
            {
                return new(resourceLoader.GetString("Scholarship_NotApplied"), "Gray");
            }
        }
    }
}
