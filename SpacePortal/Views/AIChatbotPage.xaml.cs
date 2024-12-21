using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Microsoft.SemanticKernel.ChatCompletion;
using WinUIEx.Messaging;
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;


namespace SpacePortal.Views;

public sealed partial class AIChatbotPage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    private string? _imageFilePath = null;

    public AIChatbotViewModel ViewModel
    {
        get;
    }

    public AIChatbotPage()
    {
        ViewModel = App.GetService<AIChatbotViewModel>();
        InitializeComponent();
        SetupToolTipForButton();
        AddEventForChatMessageListView();
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void SetupToolTipForButton()
    {
        ToolTipService.SetToolTip(RestartButton, resourceLoader.GetString("AIChatbotPage_RestartButton/ToolTip"));
    }

    private async void SendButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Debug.WriteLine(ChatMessagesListView.Items.Count);
        if (string.IsNullOrEmpty(ViewModel.ErrorMessage) && !string.IsNullOrWhiteSpace(InputPrompt.Text))
        {
            var userInput = InputPrompt.Text;
            InputPrompt.Text = string.Empty;
            if (!string.IsNullOrEmpty(_imageFilePath))
            {
                DroppedImage.Visibility = Visibility.Collapsed;
                await ViewModel.GetResponse(userInput, _imageFilePath);
                _imageFilePath = null;
            }
            else
            {
                await ViewModel.GetResponse(userInput);
            }
            
        }
        else if (!string.IsNullOrEmpty(ViewModel.ErrorMessage))
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
            if (!string.IsNullOrWhiteSpace(InputPrompt.Text) && e.Key == Windows.System.VirtualKey.Enter)
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
            PrimaryButtonText = resourceLoader.GetString("AIChatbotPage_RestartPrimaryButtonDialog/Text"),
            DefaultButton = ContentDialogButton.Primary
        };

        dialog.PrimaryButtonClick += (s, e) =>
        {
            ViewModel.ClearChatHistory();
        };

        await dialog.ShowAsync();
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ClearChatHistory();
    }

    private void Grid_DragOver(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }
    }

    private async void Grid_Drop(object sender, DragEventArgs e)
    {
        if (e.DataView.Contains(StandardDataFormats.StorageItems))
        {
            var items = await e.DataView.GetStorageItemsAsync();
            var storageFile = items[0] as Windows.Storage.StorageFile;

            if (storageFile != null && storageFile.ContentType.StartsWith("image/"))
            {
                _imageFilePath = storageFile.Path;
                var bitmap = new BitmapImage();
                using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    bitmap.SetSource(stream);
                }

                DroppedImage.Source = bitmap;
                DroppedImage.Visibility = Visibility.Visible;
            }
        }
    }

}
