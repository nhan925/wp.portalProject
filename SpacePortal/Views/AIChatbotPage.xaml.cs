using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class AIChatbotPage : Page
{
    public AIChatbotViewModel ViewModel
    {
        get;
    }

    public AIChatbotPage()
    {
        ViewModel = App.GetService<AIChatbotViewModel>();
        InitializeComponent();
    }
}
