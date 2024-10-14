using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class InformationPage : Page
{
    public InformationViewModel ViewModel
    {
        get;
    }

    public InformationPage()
    {
        ViewModel = App.GetService<InformationViewModel>();
        InitializeComponent();
    }
}
