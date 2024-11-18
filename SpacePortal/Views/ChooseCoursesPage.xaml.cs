using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class ChooseCoursesPage : Page
{
    public ChooseCoursesViewModel ViewModel
    {
        get;
    }

    public ChooseCoursesPage()
    {
        ViewModel = App.GetService<ChooseCoursesViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var periodInfo = e.Parameter as KeyValuePair<int, string>?;
        ContentBlock.Text = periodInfo?.Key.ToString();
        await Task.Delay(10);
    }
}
