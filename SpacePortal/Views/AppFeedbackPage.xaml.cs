using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
    }

    private void FeedbackClassificationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Do something
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        // Do something
    }
}
