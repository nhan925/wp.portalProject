using System.Xml.Serialization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class AppFeedbackPage : Page
{
    public AppFeedbackViewModel ViewModel
    {
        get;
    }

    public AppFeedbackPage()
    {
        ViewModel = App.GetService<AppFeedbackViewModel>();
        InitializeComponent();
        setupDefaultComboBox();
    }

    private void setupDefaultComboBox()
    {
        FeedbackClassificationComboBox.SelectedIndex = 0;
    }

    private void FeedbackClassificationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.changeType();
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        ResourceLoader resourceLoader = new ResourceLoader();
        string message = ViewModel.checkFeedback();
        string successMessage = resourceLoader.GetString("AppFeedback_SuccessMessageDialog/Text");
        string title = "";

        if (message == successMessage)
        {
            ViewModel.sendFeedback();
            ViewModel.EditContent = "";
            title = resourceLoader.GetString("AppFeedback_SuccessTitleDialog/Text");
        }
        else
        {
            title = resourceLoader.GetString("App_Error/Text");
        }
        ShowMessage(message, title);
    }

    private async void ShowMessage(string message, string title)
    {
        ContentDialog errorDialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = new ResourceLoader().GetString("App_Close/Text"),
            XamlRoot = this.Content.XamlRoot,
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };

        await errorDialog.ShowAsync();
    }
}
