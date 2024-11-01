using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.Helpers;
using SpacePortal.Models;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Syncfusion.UI.Xaml.DataGrid.Export;
using Syncfusion.XlsIO;
using System.Drawing;
using SpacePortal.Core.Services;

namespace SpacePortal.ViewModels;

public partial class GradesViewModel : ObservableRecipient
{
    //------------DataGrid Grades Table--------------
    private IDao<InformationsForGradesPage_GradesRow> _dao;
    public ObservableCollection<InformationsForGradesPage_GradesRow> Grades {get; set;}

    //------------ComboBox Year and Semester--------------
    public ObservableCollection<string> Years { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Semesters { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> SemestersOfLatestYear { get; set; } = new ObservableCollection<string>();
    public string LatestYear { get; set; }
    public string DefaultOption {get;set;}

    //------------TextBlock General Information--------------
    public InformationsForGradesPage_GeneralInformation GeneralInformations { get;set; } = new InformationsForGradesPage_GeneralInformation();

    public InformationsForEstimateAverageGradeDialog informationsForEstimateAverageGradeDialog { get; set; }

    public InformationsForGradesPageDao DaoForDialogs
    {
        get; set;
    }

    public GradesViewModel()
    {
        _dao = App.GetService<IDao<InformationsForGradesPage_GradesRow>>();
        Grades = _dao.GetAll();
        UpdateGeneralInformation(Grades);
        DefaultOption = new ResourceLoader().GetString("GradesPage_ComboBoxDefaultOption");
        AddYears();
        AddSemester();
        DaoForDialogs = (_dao as InformationsForGradesPageDao);
        //For latest year
        LatestYear = Grades[Grades.Count - 1].Year;
        AddSemesterOfLatestYear();
    }

    public void AddYears()
    {
        Years.Add(DefaultOption);
        foreach (var grade in Grades) {
            if (!Years.Contains(grade.Year))
            {
                Years.Add(grade.Year);
            }
        }
    }

    public void AddSemester()
    {
        Semesters.Add(DefaultOption);
        Semesters.Add("1");
        Semesters.Add("2");
        Semesters.Add("3");
    }

    private void AddSemesterOfLatestYear()
    {
        SemestersOfLatestYear.Clear();
        SemestersOfLatestYear.Add(DefaultOption);
        foreach (var grade in Grades)
        {
            if (grade.Year == LatestYear && !SemestersOfLatestYear.Contains(grade.Semester))
            {
                SemestersOfLatestYear.Add(grade.Semester);
            }
        }
    }

    public void UpdateGeneralInformation(ObservableCollection<InformationsForGradesPage_GradesRow> grades)
    {
        //-----------TextBlock General Information------------
        if(grades.Count == 0)
        {
            GeneralInformations.GpaScale_4 = 0;
            GeneralInformations.GpaScale_10 = 0;
            GeneralInformations.NumberOfCourses = 0;
            GeneralInformations.NumberOfCredits = 0;
            return;
        }
        double gpaScale_4 = 0;
        double gpaScale_10 = 0;
        var numberOfCredits = 0;
        foreach (var grade in grades)
        {
            gpaScale_4 += grade.GradeScaleFour;
            gpaScale_10 += grade.GradeScaleTen;
            numberOfCredits += grade.CourseCredit;
        }
        GeneralInformations.NumberOfCourses = grades.Count;
        GeneralInformations.GpaScale_4 = Math.Round(gpaScale_4 / grades.Count, 2);
        GeneralInformations.GpaScale_10 = Math.Round(gpaScale_10 / grades.Count, 2);
        GeneralInformations.NumberOfCredits = numberOfCredits;
    }

    public void ShowGradeByYearAndSemester(string year, string semester)
    {
        var keywords = new List<string> { year, semester };
        if (semester == DefaultOption)
        {
            keywords[1] = "";
        }
        if(year == DefaultOption)
        {
            keywords[0] = "";
        }
        Grades = new ObservableCollection<InformationsForGradesPage_GradesRow>(_dao.GetAll(null, null, keywords));
        UpdateGeneralInformation(Grades);
    }

    public async void Save(MemoryStream stream, string filename)
    {
        StorageFile stFile;

        if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".xlsx";
            savePicker.SuggestedFileName = filename;
            savePicker.FileTypeChoices.Add("Excel Documents", new List<string>() { ".xlsx" });
            var hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
            stFile = await savePicker.PickSaveFileAsync();
        }
        else
        {
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }
        if (stFile != null)
        {
            using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                //Write the compressed data from the memory to the file
                using (Stream outstream = zipStream.AsStreamForWrite())
                {
                    byte[] buffer = stream.ToArray();
                    outstream.Write(buffer, 0, buffer.Length);
                    outstream.Flush();
                }
            }
            //Launch the saved Excel file.
            await Windows.System.Launcher.LaunchFileAsync(stFile);
        }
    }

    public static void GridExportHandler(object sender, DataGridExcelExportStartOptions e)
    {
        if (e.CellType == ExportCellType.HeaderCell)
        {
            e.Style.ColorIndex = ExcelKnownColors.Grey_50_percent;
            e.Style.Font.Color = ExcelKnownColors.Black;
            e.Style.Font.Size = 12;
            e.Style.Font.Bold = true;
            e.Style.Borders[ExcelBordersIndex.EdgeBottom].LineStyle 
               = e.Style.Borders[ExcelBordersIndex.EdgeTop].LineStyle
               = e.Style.Borders[ExcelBordersIndex.EdgeLeft].LineStyle
               = e.Style.Borders[ExcelBordersIndex.EdgeRight].LineStyle 
               = ExcelLineStyle.Thin;
            e.Handled = true;
        }
        else if (e.CellType == ExportCellType.RecordCell)
        {
            //e.Style.ColorIndex = ExcelKnownColors.Grey_25_percent;
            e.Style.Borders[ExcelBordersIndex.EdgeBottom].LineStyle
               = e.Style.Borders[ExcelBordersIndex.EdgeTop].LineStyle
               = e.Style.Borders[ExcelBordersIndex.EdgeLeft].LineStyle
               = e.Style.Borders[ExcelBordersIndex.EdgeRight].LineStyle
               = ExcelLineStyle.Thin;
            e.Handled = true;
        }

        else if (e.CellType == ExportCellType.GroupCaptionCell)
        {
            e.Style.Color = Syncfusion.Drawing.Color.FromArgb(252, 159, 161);
            e.Handled = true;
        }
    }

}
