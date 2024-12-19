using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
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

    public AIChatbotViewModel()
    {
        ChatMessages = new ObservableCollection<InformationsForAIChatbot>();
    }

    public async Task GetResponse(string userInput)
    {
        ChatMessages.Add(new InformationsForAIChatbot { Message = userInput, IsUser = true });

        StringBuilder chatbotResponse = new StringBuilder();
        var chatbotMessage = new InformationsForAIChatbot { Message = string.Empty, IsUser = false };
        ChatMessages.Add(chatbotMessage);

        var response = geminiService.CallApiToChat(userInput);
        await foreach (var chunk in response)
        {
            if (chunk.Content != null)
            {
                chatbotResponse.Append(chunk.Content);
                chatbotMessage.Message = chatbotResponse.ToString();
            }
        }
        Debug.WriteLine(ChatMessages.Count);
    }
}
