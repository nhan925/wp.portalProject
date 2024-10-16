using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.Charts;
using Windows.Devices.Input;

namespace SpacePortal.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardViewModel ViewModel
    {
        get;
    }

    public DashboardPage()
    {
        ViewModel = App.GetService<DashboardViewModel>();
        InitializeComponent();

    }
}
