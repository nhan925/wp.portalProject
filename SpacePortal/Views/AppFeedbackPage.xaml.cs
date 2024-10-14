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
}
