using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;

namespace SpacePortal.Views;

public sealed partial class TuitionFeeDetailPage : Page
{
    public TuitionFeeDetailViewModel ViewModel
    {
        get;
    }

    public TuitionFeeDetailPage()
    {
        ViewModel = App.GetService<TuitionFeeDetailViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is Tuple<string, int, int, int> info)
        {
            await Task.Delay(10);
            ViewModel.LoadInformations(info.Item1, info.Item2, info.Item3, info.Item4);
        }

        CourseFeeListGrid.Visibility = Visibility.Visible;
        CourseFeeListLoading.Visibility = Visibility.Collapsed;
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
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

    private void Paymetn_Click(object sender, RoutedEventArgs e)
    {
        // TODO
    }
}
