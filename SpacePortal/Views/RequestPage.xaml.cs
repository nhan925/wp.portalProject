using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Grids;
using SpacePortal.Models;
using SpacePortal.Contracts.Services;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.UI.Xaml.Input;
using Syncfusion.UI.Xaml.Grids.ScrollAxis;

namespace SpacePortal.Views;

public sealed partial class RequestPage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    
    private string _selectedStatus = "";

    public RequestViewModel ViewModel
    {
        get;
    }

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        InitializeComponent();
        InitializeStausButton();
    }

    private void InitializeStausButton()
    {
        var menuFlyout = new MenuFlyout();
        var defaultItem = new MenuFlyoutItem { Text = resourceLoader.GetString("RequestPage_AllStatus/Text") };
        defaultItem.Click += MenuItem_Click;
        menuFlyout.Items.Add(defaultItem);
        foreach (var item in ViewModel.StatusList)
        {
            var menuItem = new MenuFlyoutItem{Text = item};
            menuItem.Click += MenuItem_Click; 
            menuFlyout.Items.Add(menuItem);
        }
        StatusButton.Flyout = menuFlyout;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is MenuFlyoutItem clickedItem)
        {
            _selectedStatus = clickedItem.Text;
        }
    }

    //Change row's height based on its content
    private void sfDataGrid_QueryRowHeight(object sender, Syncfusion.UI.Xaml.DataGrid.QueryRowHeightEventArgs e)
    {
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        var autoHeight = double.NaN;
        if (this.sfDataGrid.ColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
        {
            e.Height = Math.Max(autoHeight + 16, sfDataGrid.RowHeight + 16);
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
        PageNumber.Text = "1";

    }

    // Handle previous page button click
    private void OnPreviousPageClick(object sender, RoutedEventArgs e)
    {
        var pageNumber = Convert.ToInt32(PageNumber.Text);
        if(pageNumber > 1)
        {
            ViewModel.Load(--pageNumber);
            PageNumber.Text = pageNumber.ToString();
        }
    }

    // Handle next page button click
    private void OnNextPageClick(object sender, RoutedEventArgs e)
    {
        var pageNumber = Convert.ToInt32(PageNumber.Text);
        if (pageNumber < ViewModel.TotalPages)
        {
            ViewModel.Load(++pageNumber);
            PageNumber.Text = pageNumber.ToString();
        }
        
    }

    // Handle last page button click
    private void OnLastPageClick(object sender, RoutedEventArgs e)
    {
        ViewModel.Load(ViewModel.TotalPages);
        PageNumber.Text = ViewModel.TotalPages.ToString();
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

    private void StatusButton_Click(SplitButton sender, SplitButtonClickEventArgs args)
    {
        var defaultItem = resourceLoader.GetString("RequestPage_AllStatus/Text");
        if(_selectedStatus == defaultItem)
        {
            _selectedStatus = "";
        }
        ViewModel.Keyword = _selectedStatus;
        ViewModel.Load(1);
        PageNumber.Text = "1";
    }

    private void CatchEnter_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(PageNumber.Text))
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var pageNumber = getPageNumberValid(sender as TextBox);
                ViewModel.Load(pageNumber);
            }
        }
    }

    private int getPageNumberValid(TextBox pageTextBox)
    {
        var pageNumber = 0;
        if (int.TryParse(pageTextBox.Text, out pageNumber))
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > ViewModel.TotalPages)
            {
                pageNumber = ViewModel.TotalPages;
            }
        }
        else
        {
            pageNumber = ViewModel.CurrentPage;
        }
        PageNumber.Text = pageNumber.ToString();
        return pageNumber;
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.Load(ViewModel.CurrentPage);
        PageNumber.Text = ViewModel.CurrentPage.ToString();
    }
}