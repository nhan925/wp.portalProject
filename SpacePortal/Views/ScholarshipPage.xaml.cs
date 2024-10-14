using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class ScholarshipPage : Page
{
    public ScholarshipViewModel ViewModel
    {
        get;
    }

    public ScholarshipPage()
    {
        ViewModel = App.GetService<ScholarshipViewModel>();
        InitializeComponent();
    }
}
