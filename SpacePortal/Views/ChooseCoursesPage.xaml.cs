using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using Windows.Graphics.Printing.PrintSupport;

namespace SpacePortal.Views;

public sealed partial class ChooseCoursesPage : Page
{
    private readonly ResourceLoader resourceLoader = new();

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
        await Task.Delay(10);
        ViewModel.LoadInformations(periodInfo.Value.Key, periodInfo.Value.Value);
        
        UnregisteredCoursesList.Visibility = Visibility.Visible;
        UnregisteredCoursesListLoading.Visibility = Visibility.Collapsed;

        RegisteredCoursesList.Visibility = Visibility.Visible;
        RegisteredCoursesListLoading.Visibility = Visibility.Collapsed;

        StudiedCoursesList.Visibility = Visibility.Visible;
        StudiedCoursesListLoading.Visibility = Visibility.Collapsed;

        if (ViewModel.Informations.RegisteredCredits >= ViewModel.Informations.MaxCredits)
        {
            UnregisteredCoursesDataGrid.IsEnabled = false;
            UnregisteredCoursesList.Opacity = 0.5;
            StudiedCoursesDataGrid.IsEnabled = false;
            StudiedCoursesList.Opacity = 0.5;
        }
    }

    private void sfDataGrid_QueryRowHeight(object sender, Syncfusion.UI.Xaml.DataGrid.QueryRowHeightEventArgs e)
    {
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        var autoHeight = double.NaN;
        var datagrid = sender as SfDataGrid;
        if (datagrid.ColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
        {
            e.Height = Math.Max(autoHeight + 8, datagrid.RowHeight + 8);
            e.Handled = true;
        }
    }

    private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        ViewModel.SearchUnregisteredCourses(sender.Text);
    }

    private void NavigateToChooseClassesPage(Course course, string status)
    {
        var info = new Tuple<string, string, string, string>(ViewModel.PeriodId, course.Id, course.Name, status);
        
        var navigationService = App.GetService<INavigationService>();
        var pageKey = typeof(ChooseClassesViewModel).FullName ?? "SpacePortal.ViewModels.ChooseClassesViewModel";
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey, info);
        }
    }

    private async void showAlertDialog()
    {
        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = resourceLoader.GetString("ChooseCourses_AlertDialogTitle"),
            Content = resourceLoader.GetString("ChooseCourses_AlertDialogMessage"),
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };
        await dialog.ShowAsync();
    }

    private void UnregisteredCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
        if (ViewModel.Informations.RegisteredCredits + record.Credits > ViewModel.Informations.MaxCredits)
        {
            showAlertDialog();
            return;
        }

        var status = resourceLoader.GetString("ChooseCourses_UnregisteredStatus");
        NavigateToChooseClassesPage(record, status);
    }

    private void RegisteredCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
        var status = resourceLoader.GetString("ChooseCourses_RegisteredStatus");
        NavigateToChooseClassesPage(record, status);
    }

    private void StudiedCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
        if (ViewModel.Informations.RegisteredCredits + record.Credits > ViewModel.Informations.MaxCredits)
        {
            showAlertDialog();
            return;
        }

        var status = resourceLoader.GetString("ChooseCourses_StudiedStatus");
        NavigateToChooseClassesPage(record, status);
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }
}
