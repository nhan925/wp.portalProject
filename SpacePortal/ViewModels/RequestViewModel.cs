using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.DataAccess;
using SpacePortal.Helpers;
using SpacePortal.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SpacePortal.ViewModels;

public partial class RequestViewModel : ObservableRecipient
{
    private IDao<InformationsForRequest_RequestRow> _dao;
    public string Keyword { get; set; } = "";

    public int CurrentPage { get; set; } = 1;

    public int RowsPerPage { get; set; } = 20;

    [ObservableProperty]
    private int _totalPages = 0;

    public int TotalItems { get; set; } = 0;

    [ObservableProperty]
    private List<string> _statusList = new();

    public ObservableCollection<InformationsForRequest_RequestRow> Requests
    {
        get; set;
    }

    public RequestViewModel()
    {
        GetAllRequests();
    }

    public void GetAllRequests()
    {
        _dao = App.GetService<IDao<InformationsForRequest_RequestRow>>();
        Requests = _dao.GetAll(
        CurrentPage, RowsPerPage, new List<string> { Keyword });
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
        StatusList = (_dao as InformationsForRequestPageDao).GetAllStatusOfRequest();
    }

    public void Load(int page)
    {
        CurrentPage = page;
        GetAllRequests();
    }
}