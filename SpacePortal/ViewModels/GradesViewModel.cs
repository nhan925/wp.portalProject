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
    public ObservableCollection<InformationsForGradesPage_GradesRow> SourceData {get;}
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

    public GradesViewModel()
    {
        _dao = App.GetService<IDao<InformationsForGradesPage_GradesRow>>();
        SourceData = new ObservableCollection<InformationsForGradesPage_GradesRow>(_dao.GetAll());
        Grades = new ObservableCollection<InformationsForGradesPage_GradesRow>(SourceData);
        DefaultOption = new ResourceLoader().GetString("GradesPage_ComboBoxDefaultOption");
        AddYears(2022);
        AddSemester();
        informationsForEstimateAverageGradeDialog = (_dao as InformationsForGradesPageDao).GetInformationsForEstimateAverageGradeDialog();
        //For latest year
        LatestYear = (new List<InformationsForGradesPage_GradesRow>(SourceData)).Max(x => x.Year);
        AddSemesterOfLatestYear();
    }

    public void Init()
    {
        UpdateGeneralInformation(SourceData);
        if (Grades.Any()) { Grades.Clear();}
        foreach (var grade in SourceData)
        {
            Grades.Add(grade);
        }
    }


   
    public void AddYears(int startYear)
    {
        Years.Add(DefaultOption);
        for (int i=0;i<SourceData.Count;i++) {
            if (!Years.Contains(SourceData[i].Year))
            {
                Years.Add(SourceData[i].Year);
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
        for (int i = 0; i < SourceData.Count; i++)
        {
            if (SourceData[i].Year == LatestYear && !SemestersOfLatestYear.Contains(SourceData[i].Semester))
            {
                SemestersOfLatestYear.Add(SourceData[i].Semester);
            }
        }
    }


    /// <summary>
    /// Update the general information when the grades datagrid is updated
    /// </summary>
    /// <param name="grades"></param>
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
        int numberOfCredits = 0;
        for (int i = 0; i < grades.Count; i++)
        {
            gpaScale_4 += grades[i].GradeScaleFour;
            gpaScale_10 += grades[i].GradeScaleTen;
            numberOfCredits += grades[i].CourseCredit;
        }
        GeneralInformations.NumberOfCourses = grades.Count;
        GeneralInformations.GpaScale_4 = Math.Round(gpaScale_4 / grades.Count, 2);
        GeneralInformations.GpaScale_10 = Math.Round(gpaScale_10 / grades.Count, 2);
        GeneralInformations.NumberOfCredits = numberOfCredits;
    }

    //-----------------Grades DataGrid------------------------
    /// <summary>
    /// Change the grades datagrid by year
    /// </summary>
    /// <param name="year"></param>
    public void ShowGradeByYear(string year)
    {
        var parameters = new { p_year = year, p_semester_num = "" };
        List<InformationsForGradesPage_GradesRow> list_object = App.GetService<ApiService>().Post<List<InformationsForGradesPage_GradesRow>>("/rpc/get_course_info_by_semester", parameters);
        Grades = new ObservableCollection<InformationsForGradesPage_GradesRow>(list_object);
    }

    /// <summary>
    /// Change the grades datagrid by year and semester
    /// </summary>
    /// <param name="year"></param>
    /// <param name="semester"></param>
    public void ShowGradeByYearAndSemester(string year, string semester)
    {
        /*var gradesByYear = GetByYear(SourceData, year);
        var gradesBySemester = GetBySemester(gradesByYear, semester);
        if (Grades.Any()) { Grades.Clear(); }

        if (gradesBySemester.Any())
        {
            foreach (var grade in gradesBySemester)
            {
                Grades.Add(grade);
            }
        }
        UpdateGeneralInformation(Grades);*/
        if (semester == DefaultOption)
        { semester = ""; }
        if(year == DefaultOption) 
        { year = ""; }
        var parameters = new { p_year = year, p_semester_num = semester };
        List<InformationsForGradesPage_GradesRow> list_object = App.GetService<ApiService>().Post<List<InformationsForGradesPage_GradesRow>>("/rpc/get_course_info_by_semester", parameters);
        Grades = new ObservableCollection<InformationsForGradesPage_GradesRow>(list_object);
    }

    /// <summary>
    /// Get the grades by year
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="years"></param>
    /// <returns></returns>
    public ObservableCollection<InformationsForGradesPage_GradesRow> GetByYear(ObservableCollection<InformationsForGradesPage_GradesRow> rows, string years)
    {
        if (years == DefaultOption)
        {
            return rows;
        }
        var result = new ObservableCollection<InformationsForGradesPage_GradesRow>();
        for (var i = 0; i < rows.Count; i++)
        {
            if (rows[i].Year == years)
            {
                result.Add(rows[i]);
            }
        }
        return result;
    }

    /// <summary>
    /// Get the grades by semester
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="semester"></param>
    /// <returns></returns>
    public ObservableCollection<InformationsForGradesPage_GradesRow> GetBySemester(ObservableCollection<InformationsForGradesPage_GradesRow> rows, string semester)
    {
        if (semester == DefaultOption)
        {
            return rows;
        }
        var result = new ObservableCollection<InformationsForGradesPage_GradesRow>();
        for (var i = 0; i < rows.Count; i++)
        {
            if (rows[i].Semester == semester)
            {
                result.Add(rows[i]);
            }
        }
        return result;
    }


    /// <summary>
    /// Save the Excel file
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="filename"></param>
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

    /// <summary>
    /// Styling cells based on CellType in Excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void GridExportHandler(object sender, DataGridExcelExportStartOptions e)
    {
        if (e.CellType == ExportCellType.HeaderCell)
        {
            e.Style.ColorIndex = ExcelKnownColors.Grey_50_percent;
            e.Style.Font.Color = ExcelKnownColors.Black;
            e.Style.Font.Size = 12;
            e.Style.Font.Bold = true;
            e.Handled = true;
        }
        else if (e.CellType == ExportCellType.RecordCell)
        {
            e.Style.ColorIndex = ExcelKnownColors.Grey_25_percent;
            e.Handled = true;
        }

        else if (e.CellType == ExportCellType.GroupCaptionCell)
        {
            e.Style.Color = Syncfusion.Drawing.Color.FromArgb(252, 159, 161);
            e.Handled = true;
        }
    }

}
