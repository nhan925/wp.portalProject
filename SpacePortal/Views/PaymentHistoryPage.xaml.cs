using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;

namespace SpacePortal.Views;

public sealed partial class PaymentHistoryPage : Page
{
    public PaymentHistoryViewModel ViewModel
    {
        get;
    }

    public PaymentHistoryPage()
    {
        ViewModel = App.GetService<PaymentHistoryViewModel>();
        InitializeComponent();
    }

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

        DataGridLoadingShimmer.Visibility = Visibility.Collapsed;
        sfDataGrid.Visibility = Visibility.Visible;
    }
}
