using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Grids;
using SpacePortal.Models;
using SpacePortal.Contracts.Services;

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

    // Handle first page button click
    private void OnFirstPageClick(object sender, RoutedEventArgs e)
    {
        ViewModel.Load(1);
        ViewModel.setPageNumber();
    }

    // Handle previous page button click
    private void OnPreviousPageClick(object sender, RoutedEventArgs e)
    {
        var pageNumber = Convert.ToInt32(NumberPage1.Content);
        if (pageNumber > 1)
        {
            NumberPage2.Content = pageNumber;
            NumberPage1.Content = pageNumber - 1;
        }
    }

    // Handle page number button click
    private void OnPageClick(object sender, RoutedEventArgs e)
    {
        var pageNumber = Convert.ToInt32((sender as Button).Content);
        ViewModel.Load(pageNumber);
        ViewModel.setPageNumber();
    }

    // Handle next page button click
    private void OnNextPageClick(object sender, RoutedEventArgs e)
    {
       var pageNumber = Convert.ToInt32(NumberPage2.Content);
        if (pageNumber < ViewModel.TotalPages)
        {
            NumberPage1.Content = pageNumber;
            NumberPage2.Content = pageNumber + 1;
        }
    }

    // Handle last page button click
    private void OnLastPageClick(object sender, RoutedEventArgs e)
    {
        ViewModel.Load(ViewModel.TotalPages);
        ViewModel.setPageNumber();
    }

    private void sfDataGrid_CellDoubleTapped(object sender, GridCellDoubleTappedEventArgs e)
    {
        var record = e.Record as InformationsForRequest_RequestRow;
        var navigationService = App.GetService<INavigationService>();
        var pageKey = typeof(RequestDetailViewModel).FullName ?? 
            "SpacePortal.ViewModels.RequestDetailViewModel";
        if (pageKey != null)
        {
            navigationService.NavigateTo(pageKey, record);
        }
    }
}
