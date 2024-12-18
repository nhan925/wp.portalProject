using System.Threading;
using System.Xml.Serialization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class SchedulePage : Page
{
    private readonly ResourceLoader resourceLoader = new();
    public ScheduleViewModel ViewModel
    {
        get;
    }

    public SchedulePage()
    {
        ViewModel = App.GetService<ScheduleViewModel>();
        InitializeComponent();
        SetupDefaultForComboBoxSemester();
    }

    private void SetupDefaultForComboBoxSemester()
    {
        if(ViewModel.Semesters.Count > 0)
        {
            ComboBoxSemester.SelectedIndex = ViewModel.Semesters.Count - 1;
        }
    }

    private void ComboBoxSemester_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cbSemester = ComboBoxSemester.SelectedItem?.ToString();
        ViewModel.ShowScheduleBySemester(cbSemester);
    }

    private async void Schedule_AppointmentTapped(object sender, Syncfusion.UI.Xaml.Scheduler.AppointmentTappedArgs e)
    {
        if (e.Appointment != null && e.Appointment.Data is InformationsForSchedulePage_Class tappedAppointment)
        {
            var lecturer = tappedAppointment.Lecturer;

            var container = new StackPanel() { Orientation = Orientation.Vertical };
            var content = new TextBlock
            {
                Text = $"{resourceLoader.GetString("SchedulePage_FullNameLecturer/Text")}: {lecturer.FullName}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_GenderLecturer/Text")}: {lecturer.Gender}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_PhoneNumberLecturer/Text")}: {lecturer.PhoneNumber}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_EmailLecturer/Text")}: {lecturer.Email}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_AcademicRankLecturer/Text")}: {lecturer.AcademicRank}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_AcademicDegreeLecturer/Text")}: {lecturer.AcademicDegree}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_FacultyLecturer/Text")}: {lecturer.FacultyName}\n\n" +
                                   $"{resourceLoader.GetString("SchedulePage_CourseOutline/Text")}: ",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Left
            };
            var hyperlink = new Hyperlink
            {
                Inlines =
                {
                    new Run
                    {
                        Text = resourceLoader.GetString("SchedulePage_CourseOutlineLink/Text"),
                    }
                }
            };
            hyperlink.Click += (s, e) =>
            {
                OpenWebView(tappedAppointment.CourseUrl);
            };

            content.Inlines.Add(hyperlink);
            container.Children.Add(content);


            ContentDialog lecturerDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = resourceLoader.GetString("SchedulePage_InformationOfLecturer/Text"),
                Content = container,
                CloseButtonText = resourceLoader.GetString("App_Close/Text")
            };

            lecturerDialog.RequestedTheme = App.GetService<IThemeSelectorService>().Theme;
            await lecturerDialog.ShowAsync();
        }
    }

    private async void OpenWebView(String url)
    {
        var webView2 = new WebView2();
        await webView2.EnsureCoreWebView2Async();

        webView2.Source = new Uri(url);

        // Disable the right-click menu
        webView2.CoreWebView2.ContextMenuRequested += (sender, e) =>
        {
            e.Handled = true;
        };

        var newWindow = new WindowEx
        {
            Title = resourceLoader.GetString("SchedulePage_CourseOutline/Text"),
            Content = webView2,
            MinWidth = 1100,
            MinHeight = 600,
            SystemBackdrop = new MicaBackdrop(),
        };
        newWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        newWindow.CenterOnScreen();

        newWindow.Activate();
    }
}
