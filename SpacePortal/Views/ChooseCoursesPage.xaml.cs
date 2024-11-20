using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;

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
        await Task.Delay(10);
        ViewModel.LoadInformations(periodInfo.Value.Key, periodInfo.Value.Value);
        
        UnregisteredCoursesList.Visibility = Visibility.Visible;
        UnregisteredCoursesListLoading.Visibility = Visibility.Collapsed;

        RegisteredCoursesList.Visibility = Visibility.Visible;
        RegisteredCoursesListLoading.Visibility = Visibility.Collapsed;

        StudiedCoursesList.Visibility = Visibility.Visible;
        StudiedCoursesListLoading.Visibility = Visibility.Collapsed;
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

    private void UnregisteredCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
        
    }

    private void RegisteredCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
    }

    private void StudiedCoursesList_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as Course;
    }
}
