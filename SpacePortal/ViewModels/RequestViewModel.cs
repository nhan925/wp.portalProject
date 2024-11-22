using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Helpers;
using SpacePortal.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SpacePortal.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    public int CurrentPage { get; set; } = 1;

    public int RowsPerPage { get; set; } = 20;

    public int TotalPages { get; set; } = 0;

    public int TotalItems { get; set; } = 0;

    [ObservableProperty]
    private string _pageNumber;

    public ObservableCollection<InformationsForRequest_RequestRow> Requests
    {
        get; set;
    }

    public RequestViewModel()
    {
        GetAllRequests();
        setPageNumber();
    }

    public void GetAllRequests()
    {
        Requests = App.GetService<IDao<InformationsForRequest_RequestRow>>().GetAll(
        CurrentPage, RowsPerPage);
        var startSequence = (CurrentPage - 1) * RowsPerPage;
        for (int i = 0; i < Requests.Count; i++)
        {
            Requests[i].SequenceNumber = ++startSequence;
        }

        if (Requests.Count != 0)
        {
            TotalItems = Requests[0].TotalRequests;
            TotalPages = (TotalItems / RowsPerPage)
                + ((TotalItems % RowsPerPage == 0)
                        ? 0 : 1);
        }
    }

    public void Load(int page)
    {
        CurrentPage = page;
        GetAllRequests();
    }

    public void setPageNumber()
    {
        PageNumber = $"{new ResourceLoader().GetString("RequestPage_PageNumber/Text")} {CurrentPage} / {TotalPages}";
    }
}