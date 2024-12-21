﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

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

    public async Task<StorageFile?> AttachImageAsync()
    {
        FileOpenPicker fileOpenPicker = new()
        {
            ViewMode = PickerViewMode.Thumbnail,
            FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif" },
        };

        nint windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);

        StorageFile file = await fileOpenPicker.PickSingleFileAsync();
        if (file != null)
        {
            if (await IsValidFileAsync(file))
            {
                return file;
            }
        }

        return null;
    }

    private async Task<bool> IsValidFileAsync(StorageFile file)
    {
        const ulong MaxFileSizeInBytes = 5 * 1024 * 1024;

        var properties = await file.GetBasicPropertiesAsync();
        return properties.Size <= MaxFileSizeInBytes;
    }
}
