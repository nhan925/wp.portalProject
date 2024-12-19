using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Microsoft.SemanticKernel.ChatCompletion;


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

    private async void SendButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var userInput = InputPrompt.Text;
        InputPrompt.Text = string.Empty;
        await ViewModel.GetResponse(userInput);
    }
}
