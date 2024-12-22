using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.DataAccess;
using SpacePortal.Helpers;
using SpacePortal.Models;
using Syncfusion.XlsIO.Implementation;

namespace SpacePortal.ViewModels;

public partial class ScholarshipViewModel : ObservableRecipient
{
    private readonly ResourceLoader resourceLoader = new();

    private ObservableCollection<InformationsForScholarshipPage> _informations
    {
        get; set;
    }

    public ObservableCollection<InformationsForScholarshipPage> Informations
    {
        get; set;
    }

    public InformationsForScholarshipPage SelectingItem { get; set; } = new InformationsForScholarshipPage();

    public List<string> Years
    {
        get {
            var years = new List<string>();
            years.Add(resourceLoader.GetString("Scholarship_AllYear"));
            foreach (var date in Informations.Select(x => x.AnouncementDate.Year).Distinct())
            {
                years.Add(date.ToString());
            }
            return years;
        }
    }

    public ScholarshipViewModel()
    {
        LoadData("");
    }
        

    public void LoadData(string year, string keyword = "")
    {
        _informations = (App.GetService<IDao<InformationsForScholarshipPage>>() as InformationsForScholarshipPageDao).GetByYear(year);
        SearchScholarships(keyword);
    }

    // Full-text search
    public void SearchScholarships(string search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = Regex.Replace(search.Trim(), @"\s+", " ").NormalizeSearch();
            Informations = new ObservableCollection<InformationsForScholarshipPage>(
                _informations.Where(s => s.Name.NormalizeSearch().Contains(normalizedSearch))
            );
        }
        else
        {
            Informations = _informations;
        }
    }

    public async Task<bool> ApplyScholarship(string filePath)
    {
        var url = await App.GetService<SupabaseFileService>().UploadFileAsync(filePath);
        if (url != null)
        {
            var result = App.GetService<ApiService>().Post<string>("/rpc/apply_scholarship", new
            {
                id = SelectingItem.Id,
                file_url = url
            });

            if (result == "SUCCESS") { return true; }
            else { return false; }
        }
        return false;
    }
}
