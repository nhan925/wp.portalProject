using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.ViewModels;

public partial class AIChatbotViewModel : ObservableRecipient
{
    public GeminiService geminiService => App.GetService<GeminiService>();

    public ObservableCollection<InformationsForAIChatbot> ChatMessages
    {
        get; set;
    }

    [ObservableProperty]
    private string _errorMessage;

    public AIChatbotViewModel()
    {
        ChatMessages = new ObservableCollection<InformationsForAIChatbot>();
        ErrorMessage = string.Empty;
    }

    public async Task GetResponse(string userInput, string? imageFilePath = null)
    {

        try
        {
            ChatMessages.Add(new InformationsForAIChatbot { Message = userInput, IsUser = true });

            StringBuilder chatbotResponse = new StringBuilder();
            var chatbotMessage = new InformationsForAIChatbot { Message = string.Empty, IsUser = false };
            ChatMessages.Add(chatbotMessage);

            var response = geminiService.CallApiToChat(userInput,imageFilePath);
            await foreach (var chunk in response)
            {
                if (chunk.Content != null)
                {
                    chatbotResponse.Append(chunk.Content);
                    chatbotMessage.Message = chatbotResponse.ToString();
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public void ClearChatHistory()
    {
        ChatMessages.Clear();
        ErrorMessage = string.Empty;
        geminiService.ClearChatHistory();
    }

}
