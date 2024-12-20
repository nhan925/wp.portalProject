using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Microsoft.SemanticKernel.ChatCompletion;
using WinUIEx.Messaging;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;


namespace SpacePortal.Views;

public sealed partial class AIChatbotPage : Page
{
    private readonly ResourceLoader resourceLoader = new();

    public AIChatbotViewModel ViewModel
    {
        get;
    }

    public AIChatbotPage()
    {
        ViewModel = App.GetService<AIChatbotViewModel>();
        InitializeComponent();
        AddEventForChatMessageListView();
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private async void SendButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Debug.WriteLine(ChatMessagesListView.Items.Count);
        if (string.IsNullOrEmpty(ViewModel.ErrorMessage))
        {
            var userInput = InputPrompt.Text;
            InputPrompt.Text = string.Empty;
            await ViewModel.GetResponse(userInput);
        }
        else
        {
            InputPrompt.Text = string.Empty;
            var errorMessage = resourceLoader.GetString("AIChatbotPage_ErrorMessage/Text");
            ShowErrorDialog(errorMessage);
        }
    }

    private void AddEventForChatMessageListView()
    {
        ViewModel.ChatMessages.CollectionChanged += (s, e) =>
        {
            Debug.WriteLine("Items_CollectionChanged called");
            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                DispatcherQueue.TryEnqueue(() =>
                {
                    if (ChatMessagesListView.Items.Count > 0)
                    {
                        ChatMessagesListView.UpdateLayout();
                        ChatMessagesListView.ScrollIntoView(ChatMessagesListView.Items[^1], ScrollIntoViewAlignment.Default);
                        if (ChatMessagesListView.Visibility == Visibility.Collapsed)
                        {
                            HelperText.Visibility = Visibility.Collapsed;
                            ChatMessagesListView.Visibility = Visibility.Visible;
                        }
                    }
                });
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    HelperText.Visibility = Visibility.Visible;
                    ChatMessagesListView.Visibility = Visibility.Collapsed;
                });
            }
        };
    }

    private void InputPrompt_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
            if (!string.IsNullOrEmpty(InputPrompt.Text) && e.Key == Windows.System.VirtualKey.Enter)
            {
                SendButton_Click(sender, null);
            }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.ErrorMessage) && !string.IsNullOrEmpty(ViewModel.ErrorMessage))
        {
            var errorMessage = resourceLoader.GetString("AIChatbotPage_ErrorMessage/Text");
            ShowErrorDialog(errorMessage);
        }
    }

    private async Task ShowErrorDialog(string errorMessage)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = resourceLoader.GetString("AIChatbotPage_HelloText/Text"),
            Content = errorMessage,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            PrimaryButtonText = resourceLoader.GetString("AIChatbotPage_RestartButton/Text"),
            DefaultButton = ContentDialogButton.Primary
        };

        dialog.PrimaryButtonClick += (s, e) =>
        {
            ViewModel.ClearChatHistory();
        };

        await dialog.ShowAsync();
    }
}
