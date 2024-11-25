using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class AppFeedbackViewModel : ObservableRecipient
{
    public ObservableCollection<string> TypeItemComboBox { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    private string _editContent;

    private string _type;

    public AppFeedbackViewModel()
    {
        LoadData();
    }

    public void LoadData()
    {
        string type1 = new ResourceLoader().GetString("AppFeedback_FeedbackType/Text");
        string type2 = new ResourceLoader().GetString("AppFeedback_ErrorType/Text");
        TypeItemComboBox.Add(type1);
        TypeItemComboBox.Add(type2);
        EditContent = "";
        _type = "";
    }

    public void changeType()
    {
        if (_type == TypeItemComboBox[0])
        {
            _type = TypeItemComboBox[1];
        }
        else
        {
            _type = TypeItemComboBox[0];
        }
    }

    public string sendFeedback()
    {
        var content = EditContent;
        var type = _type;
        var feedbackObject = new
        {
            content,
            type
        };
        string result = App.GetService<ApiService>().Post<string>("/rpc/add_feedback", feedbackObject);
        return result;
    }

    public string checkFeedback()
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        if (string.IsNullOrWhiteSpace(EditContent))
        {
            return resourceLoader.GetString("AppFeedback_EmptyFeedback/Text");
        }
        if (EditContent.Length < 10)
        {
            return resourceLoader.GetString("AppFeedback_FeedbackTooShort/Text"); 
        }
        if (EditContent.Length > 500)
        {
            return resourceLoader.GetString("AppFeedback_FeedbackTooLong/Text"); 
        }
        return resourceLoader.GetString("AppFeedback_SuccessMessageDialog/Text");
    }
}