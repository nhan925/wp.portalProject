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
using Windows.Storage.Pickers;
using static System.Net.Mime.MediaTypeNames;
using SpacePortal.Contracts.Services;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;


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
        ToolTipService.SetToolTip(PickAPhotoButton, resourceLoader.GetString("AIChatbotPage_AddImage/ToolTip"));
    }

    private async void SendButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(ViewModel.ErrorMessage) && !string.IsNullOrWhiteSpace(InputPrompt.Text))
        {
            InputPrompt.IsEnabled = false;
            SendButton.IsEnabled = false;
            RestartButton.IsEnabled = false;
            PickAPhotoButton.IsEnabled = false;
            ToolBarArea.Opacity = 0.5;
            ThinkingBar.Visibility = Visibility.Visible;
            await Task.Delay(10);

            var userInput = InputPrompt.Text;
            InputPrompt.Text = string.Empty;
            if (!string.IsNullOrEmpty(_imageFilePath))
            {
                ImageArea.Visibility = Visibility.Collapsed;
                userInput += $"\n[{resourceLoader.GetString("AIChatbotPage_Image/Text")}]";
                await ViewModel.GetResponse(userInput, _imageFilePath);
                _imageFilePath = null;
            }
            else
            {
                await ViewModel.GetResponse(userInput);
            }

            ThinkingBar.Visibility = Visibility.Collapsed;
            ToolBarArea.Opacity = 1;
            SendButton.IsEnabled = true;
            RestartButton.IsEnabled = true;
            PickAPhotoButton.IsEnabled = true;
            InputPrompt.IsEnabled = true;
            InputPrompt.Focus(FocusState.Programmatic);

        }
        else if (!string.IsNullOrEmpty(ViewModel.ErrorMessage))
        {
            InputPrompt.Text = string.Empty;
            var errorMessage = resourceLoader.GetString("AIChatbotPage_ErrorMessage/Text");
            await ShowErrorDialog(errorMessage);
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
            SendButton_Click(sender, new());
        }
    }

    private async void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.ErrorMessage) && !string.IsNullOrEmpty(ViewModel.ErrorMessage))
        {
            var errorMessage = resourceLoader.GetString("AIChatbotPage_ErrorMessage/Text");
            await ShowErrorDialog(errorMessage);
        }
    }

    private async Task ShowErrorDialog(string errorMessage, bool hasPrimaryButton = true)
    {
        var dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = resourceLoader.GetString("AIChatbotPage_DialogTitle");
        dialog.Content = errorMessage;
        dialog.CloseButtonText = resourceLoader.GetString("App_Close/Text");
        if (hasPrimaryButton)
        {
            dialog.PrimaryButtonText = resourceLoader.GetString("AIChatbotPage_RestartPrimaryButtonDialog/Text");
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.PrimaryButtonClick += (s, e) =>
            {
                ViewModel.ClearChatHistory();
            };
        }
        dialog.RequestedTheme = App.GetService<IThemeSelectorService>().Theme;
        await dialog.ShowAsync();
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.ChatMessages.Count > 0)
        {
            ViewModel.ClearChatHistory();
            InputPrompt.Text = string.Empty;

            showNotifications(resourceLoader.GetString("AIChatbotPage_DialogTitle"),
                resourceLoader.GetString("AIChatbotPage_ResetSuccessfull"));
        }
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
                if (await ViewModel.IsValidFileAsync(storageFile))
                {
                    _imageFilePath = storageFile.Path;
                    var bitmap = new BitmapImage();
                    using (var stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        bitmap.SetSource(stream);
                    }

                    DroppedImage.Source = bitmap;
                    ImageArea.Visibility = Visibility.Visible;
                }
                else
                {
                    await ShowErrorDialog(resourceLoader.GetString("AIChatbotPage_ErrorImage/Text"), false);
                }
            }
        }
    }

    private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
    {
        var senderButton = sender as Button;
        senderButton.IsEnabled = false;

        var file = await ViewModel.AttachImageAsync();
        if (file != null)
        {
            if (await ViewModel.IsValidFileAsync(file))
            {
                _imageFilePath = file.Path;
                var bitmap = new BitmapImage();
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    bitmap.SetSource(stream);
                }
                DroppedImage.Source = bitmap;
                ImageArea.Visibility = Visibility.Visible;
            }
            else
            {
                await ShowErrorDialog(resourceLoader.GetString("AIChatbotPage_ErrorImage/Text"), false);
            }
        }

        senderButton.IsEnabled = true;
    }

    private void showNotifications(string title, string message)
    {
        var toastBuilder = new AppNotificationBuilder()
            .AddText(title)
            .AddText(message);

        var toast = toastBuilder.BuildNotification();
        AppNotificationManager.Default.Show(toast);
    }

    private void DeleteImage_Click(object sender, RoutedEventArgs e)
    {
        ImageArea.Visibility = Visibility.Collapsed;
        _imageFilePath = null;
    }
}
