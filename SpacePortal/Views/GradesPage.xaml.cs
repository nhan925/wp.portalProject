using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class GradesPage : Page
{
    public GradesViewModel ViewModel
    {
        get;
    }

    public GradesPage()
    {
        ViewModel = App.GetService<GradesViewModel>();
        InitializeComponent();
    }
}
