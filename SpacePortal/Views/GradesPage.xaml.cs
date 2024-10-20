using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.ViewModels;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.WebUI;
using Syncfusion.UI.Xaml.DataGrid.Export;
using Microsoft.Extensions.Options;
using Syncfusion.XlsIO;
using System.IO;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Xml.Serialization;



namespace SpacePortal.Views;

public sealed partial class GradesPage : Page
{
    public event EventHandler? SelectedIndexChanged;
    public GradesViewModel ViewModel
    {
        get;
    }

    public GradesPage()
    {
        this.InitializeComponent();
        ViewModel = new GradesViewModel();
        SetupDafaultComboBox();
        ViewModel.Init();
    }

    private void SetupDafaultComboBox()
    {
        ComboBoxYear.SelectedItem = ViewModel.DefaultOption;
        ComboBoxSemester.SelectedItem = ViewModel.DefaultOption;
    }

    private void ShowGradeButton_Click(object sender, RoutedEventArgs e)
    {
        var cbYear = ComboBoxYear.SelectedItem?.ToString();

        if (cbYear == ViewModel.DefaultOption)
        {
            ComboBoxSemester.SelectedItem = ViewModel.DefaultOption;
            ViewModel.Init();
            return;
        }
        var cbSemester = ComboBoxSemester.SelectedItem?.ToString();

        if (cbYear != null && cbSemester != null)
        {
            ViewModel.ShowGradeByYearAndSemester(cbYear, cbSemester);
        }
        //Just for precautions
        else if (cbYear != null)
        {
            ViewModel.ShowGradeByYear(cbYear);
        }
        else
        {
            ViewModel.Init();
        }
    }


    private void CalculatorButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RequestReviewMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RequestForTranscriptMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {

    }

    private void RequestExportExcelMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        var options = new DataGridExcelExportOptions();
        options.ExportMode = ExportMode.Text;
        options.GridExportHandler = GradesViewModel.GridExportHandler;
        var excelEngine = sfDataGrid.ExportToExcel(sfDataGrid.View, options);
        var workBook = excelEngine.Excel.Workbooks[0];
        MemoryStream stream = new MemoryStream();
        workBook.SaveAs(stream);
        ViewModel.Save(stream, "Grades.xlsx");
    }

    private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(ComboBoxYear.SelectedItem == ViewModel.LatestYear)
        {
            ViewModel.Semesters.Clear();
            foreach (var semester in ViewModel.SemestersOfLatestYear)
            {
                ViewModel.Semesters.Add(semester);
            }
           
        }
        else
        {
            ViewModel.Semesters.Clear();
            ViewModel.AddSemester();
        }
        ComboBoxSemester.SelectedItem = ViewModel.DefaultOption;
    }
}
