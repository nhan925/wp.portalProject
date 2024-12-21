using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Windows.Storage.Pickers;
using Windows.UI;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.DataGrid;
using Microsoft.UI.Xaml.Documents;
using System.ComponentModel;
using CommunityToolkit.WinUI;
using Windows.UI.ViewManagement;

namespace SpacePortal.Views;

public sealed partial class ScholarshipPage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    public ScholarshipViewModel ViewModel
    {
        get;
    }

    public ScholarshipPage()
    {
        ViewModel = App.GetService<ScholarshipViewModel>();
        InitializeComponent();
    }

    private async void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadingOverlay.Visibility = Visibility.Visible;
        splitView.Opacity = 0.5;
        await Task.Delay(10);

        DetailTextBox.Visibility = Visibility.Collapsed;
        RegisterButton.IsEnabled = false;
        SearchBox.Text = "";
        var comboBox = sender as ComboBox;
        if (comboBox.SelectedIndex == 0)
        {
            ViewModel.LoadData("");
        }
        else
        {
            ViewModel.LoadData(comboBox.SelectedItem.ToString());
        }
        LoadingOverlay.Visibility = Visibility.Collapsed;
        splitView.Opacity = 1;
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = (sender as ListView).SelectedItem as InformationsForScholarshipPage;
        if (item != null)
        {
            DetailTextBox.Visibility = Visibility.Visible;
            if (!item.Applied)
            {
                RegisteredInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                RegisteredInfo.Visibility = Visibility.Visible;
            }
            ViewModel.SelectingItem = item;

            if (item.Expired)
            {
                RegisterButton.IsEnabled = false;
            }
            else
            {
                RegisterButton.IsEnabled = !item.Applied;
            }
        }
    }

    private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        DetailTextBox.Visibility = Visibility.Collapsed;
        RegisterButton.IsEnabled = false;
        ViewModel.SearchScholarships(SearchBox.Text);
    }

    private void showNotifications(string title, string message)
    {
        var toastBuilder = new AppNotificationBuilder()
            .AddText(title)
            .AddText(message);

        var toast = toastBuilder.BuildNotification();
        AppNotificationManager.Default.Show(toast);
    }

    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        LoadingOverlay.Visibility = Visibility.Visible;
        splitView.Opacity = 0.5;
        await Task.Delay(10);

        // TextBlock to display the selected file name
        var fileNameTextBlock = new TextBlock
        {
            Text = resourceLoader.GetString("Scholarship_DialogNoFile"),
            FontStyle = Windows.UI.Text.FontStyle.Italic,
            Foreground = new SolidColorBrush(Colors.Red)
        };

        // File picker button
        var filePickerButton = new Button
        {
            Content = new FontIcon { Glyph = "\uE8E5" },
            HorizontalAlignment = HorizontalAlignment.Right,
        };

        var container = new Grid
        {
            Margin = new Thickness(0, 16, 0, 0),
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            ColumnSpacing = 16,
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(fileNameTextBlock, 0);
        container.Children.Add(fileNameTextBlock);
        Grid.SetColumn(filePickerButton, 1);
        container.Children.Add(filePickerButton);

        var filePath = string.Empty;

        filePickerButton.Click += async (s, e) =>
        {
            filePickerButton.IsEnabled = false;
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".pdf");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                filePath = file.Path;
                fileNameTextBlock.Text = $"{file.Name}";
                fileNameTextBlock.FontStyle = Windows.UI.Text.FontStyle.Normal; // Reset font style
            }
            else
            {
                filePath = string.Empty;
                fileNameTextBlock.Text = resourceLoader.GetString("Scholarship_DialogNoFile"); // Reset if no file chosen
                fileNameTextBlock.FontStyle = Windows.UI.Text.FontStyle.Italic;
            }

            filePickerButton.IsEnabled = true;
        };

        // Create dialog content
        var dialogContent = new StackPanel();
        dialogContent.Children.Add(new TextBlock
        {
            Text = resourceLoader.GetString("Scholarship_DialogContent"),
        });
        dialogContent.Children.Add(container);

        // Create the dialog
        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme,
            Title = resourceLoader.GetString("Scholarship_DialogTitle"),
            Content = dialogContent,
            PrimaryButtonText = resourceLoader.GetString("Scholarship_RegisterButton/Content"),
            CloseButtonText = resourceLoader.GetString("App_Close/Text")
        };

        dialog.PrimaryButtonClick += async (s, e) =>
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var result = await ViewModel.ApplyScholarship(filePath);
                if (result)
                {
                    showNotifications(resourceLoader.GetString("Scholarship_Title/Text"),
                        resourceLoader.GetString("Scholarship_DialogApplySuccess"));

                    var year = "";
                    if (YearComboBox.SelectedIndex > 0)
                    {
                        year = ViewModel.Years[YearComboBox.SelectedIndex];
                    }
                    else
                    {
                        year = "";
                    }
                    var index = ScholarshipListView.SelectedIndex;
                    ViewModel.LoadData(year, SearchBox.Text);
                    ScholarshipListView.SelectedIndex = index;
                }
                else
                {
                    showNotifications(resourceLoader.GetString("Scholarship_Title/Text"),
                        resourceLoader.GetString("Scholarship_DialogApplyFailed"));
                }
                LoadingOverlay.Visibility = Visibility.Collapsed;
                splitView.Opacity = 1;
            }
            else
            {
                e.Cancel = true;
            }
        };

        dialog.CloseButtonClick += (s, e) =>
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
            splitView.Opacity = 1;
        };

        await dialog.ShowAsync();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        LoadingOverlay.Visibility = Visibility.Collapsed;
        splitView.Visibility = Visibility.Visible;
    }

    private async void Hyperlink_Click(Microsoft.UI.Xaml.Documents.Hyperlink sender, Microsoft.UI.Xaml.Documents.HyperlinkClickEventArgs args)
    {
        var url = ViewModel.SelectingItem.Attachment;

        if (!string.IsNullOrEmpty(url))
        {
            var webView2 = new WebView2();
            await webView2.EnsureCoreWebView2Async();

            webView2.Source = new Uri(url);
            
            // Disable the right-click menu
            webView2.CoreWebView2.ContextMenuRequested += (sender, e) =>
            {
                e.Handled = true;
            };

            var newWindow = new WindowEx
            {
                Title = resourceLoader.GetString("Scholarship_DocumentWindowsTitle"),
                Content = webView2,
                MinWidth = 1100,
                MinHeight = 600,
                SystemBackdrop = new MicaBackdrop(),
            };
            newWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
            newWindow.CenterOnScreen();

            newWindow.Activate();
        }
    }
}
