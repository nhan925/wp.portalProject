using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class RequestDetailViewModel : ObservableRecipient
{
    IDao<InformationsForRequestDetailPage_Answer> _dao;

    ResourceLoader resourceLoader = new ResourceLoader();

    [ObservableProperty]
    private string _formattedRequestContent = string.Empty;

    public InformationsForRequest_RequestRow Request { set;  get; }

    public InformationsForRequestDetailPage_Answer Answer { set; get; }

    public RequestDetailViewModel()
    {
        _dao = App.GetService<IDao<InformationsForRequestDetailPage_Answer>>();
        Answer = new InformationsForRequestDetailPage_Answer();
    }

    public void FormatRequestContent()
    {
        var requestReExamiantion = resourceLoader.GetString("GradesPage_RequestForReviewTitle");
        var requestPhysicalTranscript = resourceLoader.GetString("GradesPage_RequestForTranscriptTitle");
        var student = resourceLoader.GetString("RequestDetailPage_Student/Text");
        StringBuilder requestContent = new();

        var lines = Request.Content.Split("\r\n");
        if (lines[0] == requestReExamiantion)
        {
            lines[0] = $"{student} {Request.StudentName} {lines[0].ToLower()}: \n";
            requestContent.Append(string.Join("\t + ", lines.Select(line => line + "\n")));
        }
        else if (lines[0] == requestPhysicalTranscript)
        {
            lines[0] = $"{student} {Request.StudentName} {lines[0].ToLower()}:";
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                requestContent.Append(string.Join("\t + ", parts.Select(part => part + "\n")));
                requestContent.Append("\n");
            }
        }
        FormattedRequestContent = requestContent.ToString();
    }

    public string ResendRequest()
    {
        var processingRequest = resourceLoader.GetString("RequestPage_ProcessingStatus/Text");
        var parameters = new { content = Request.Content, status = processingRequest };
        var result = App.GetService<ApiService>().Post<string>("/rpc/add_request", parameters);
        return resourceLoader.GetString("RequestDetailPage_ResendRequestSuccess/Text");
    }

    public string CancelledRequest()
    {
        var cancelledRequest = resourceLoader.GetString("RequestPage_CancelledStatus/Text");
        var parameters = new { request_id_update = Request.RequestId, status_update = cancelledRequest };
        var result = App.GetService<ApiService>().Post<string>("/rpc/update_request_status", parameters);
        return resourceLoader.GetString("RequestDetailPage_CancelledRequestSuccess/Text");
    }

    public void GetAnswer()
    {
        if(Request != null)
        {
            if(Request.Status == resourceLoader.GetString("RequestPage_CompletedStatus/Text"))
            {
                Answer = _dao.GetById(Request.RequestId.ToString());
            }
            else
            {
                Answer.AnswerId = 0;
                Answer.AdminName = resourceLoader.GetString("RequestDetailPage_Anonymous/Text"); 
                Answer.AnswerContent = Request.Status;
                Answer.SubmittedAt = new DateTime(1999, 1, 1); 
            }
        }
        
    }
}
