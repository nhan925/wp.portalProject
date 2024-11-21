using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Grids;

namespace SpacePortal.Views;

public sealed partial class RequestPage : Page
{
    public RequestViewModel ViewModel
    {
        get;
    }

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        InitializeComponent();

        this.sfDataGrid.SortColumnsChanging += SfDataGrid_SortColumnsChanging;
    }

    //Change row's height based on its content
    private void sfDataGrid_QueryRowHeight(object sender, Syncfusion.UI.Xaml.DataGrid.QueryRowHeightEventArgs e)
    {
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        var autoHeight = double.NaN;
        if (this.sfDataGrid.ColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
        {
            e.Height = Math.Max(autoHeight + 8, sfDataGrid.RowHeight + 8);
            e.Handled = true;
        }
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);

        DataGridLoadingOverlay.Visibility = Visibility.Collapsed;
        sfDataGrid.Visibility = Visibility.Visible;
    }

    private void SfDataGrid_SortColumnsChanging(object? sender, GridSortColumnsChangingEventArgs e)
    {
        if (e.AddedItems[0].ColumnName == "SubmittedAt")
            e.Cancel = true;
    }
}
