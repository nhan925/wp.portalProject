using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;

namespace SpacePortal.Views;

public sealed partial class ChooseClassesPage : Page
{
    public ChooseClassesViewModel ViewModel
    {
        get;
    }

    public ChooseClassesPage()
    {
        ViewModel = App.GetService<ChooseClassesViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var periodInfo = e.Parameter as Tuple<string, string, string, string>;
        await Task.Delay(10);
        ViewModel.LoadInformations(periodInfo.Item1, periodInfo.Item2, periodInfo.Item3, periodInfo.Item4);

        ClassesListLoading.Visibility = Visibility.Collapsed;
        ClassesList.Visibility = Visibility.Visible;

        if (ViewModel.Informations.RegisteredClassId != null)
        {
            ClassDataGrid.SelectedIndex = ViewModel.Informations.Classes.FindIndex(c => c.Id == ViewModel.Informations.RegisteredClassId);
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

    private void ClassesList_CellDoubleTapped(object sender, Syncfusion.UI.Xaml.DataGrid.GridCellDoubleTappedEventArgs e)
    {

    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Register_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
