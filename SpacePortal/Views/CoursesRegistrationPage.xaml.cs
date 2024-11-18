using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class CoursesRegistrationPage : Page
{
    public CoursesRegistrationViewModel ViewModel
    {
        get;
    }

    public CoursesRegistrationPage()
    {
        ViewModel = App.GetService<CoursesRegistrationViewModel>();
        InitializeComponent();
    }

    private void PeriodsListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        ListViewLoadingOverlay.Visibility = Visibility.Collapsed;
        PeriodsListView.Visibility = Visibility.Visible;
    }
}
