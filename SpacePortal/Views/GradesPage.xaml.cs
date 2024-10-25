using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.Models;
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
    private readonly ResourceLoader resourceLoader = new();

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

    private async void CalculatorButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = resourceLoader.GetString("GradesPage_EstimateAverageGradeTitle");
        dialog.Content = new EstimateAverageGradeDialog(ViewModel.SourceData,
            ViewModel.informationsForEstimateAverageGradeDialog);
        dialog.HorizontalAlignment = HorizontalAlignment.Left;
        dialog.CloseButtonText = resourceLoader.GetString("Dialog_Cancel");
        dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;

        await dialog.ShowAsync();
    }

    private async void RequestForTranscript_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = resourceLoader.GetString("GradesPage_RequestForTranscriptTitle");
        dialog.Content = new RequestPhysicalTranscriptDialog(ViewModel.SourceData);
        dialog.HorizontalAlignment = HorizontalAlignment.Left;
        dialog.CloseButtonText = resourceLoader.GetString("Dialog_Cancel");
        dialog.PrimaryButtonText = resourceLoader.GetString("Dialog_Send");
        dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.PrimaryButtonClick += (sender, e) =>
        {
            (dialog.Content as RequestPhysicalTranscriptDialog).PrimaryButton_Click(sender, e);
        };

        await dialog.ShowAsync();
    }

    private async void RequestReview_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Title = resourceLoader.GetString("GradesPage_RequestForReviewTitle");
        dialog.Content = new RequestReExaminationDialog(ViewModel.SourceData);
        dialog.HorizontalAlignment = HorizontalAlignment.Left;
        
        
        dialog.PrimaryButtonText = resourceLoader.GetString("Dialog_Send");
        dialog.CloseButtonText = resourceLoader.GetString("Dialog_Cancel");
        dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.PrimaryButtonClick += (s, e) =>
        {
            var requestReExaminationDialog = dialog.Content as RequestReExaminationDialog;
            requestReExaminationDialog.SendRequest(s, e);
        };


        await dialog.ShowAsync();


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
