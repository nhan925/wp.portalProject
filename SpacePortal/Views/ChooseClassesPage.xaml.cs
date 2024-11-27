using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using SpacePortal.Contracts.Services;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using SpacePortal.Models;

namespace SpacePortal.Views;

public sealed partial class ChooseClassesPage : Page
{
    private readonly ResourceLoader resourceLoader = new();

    public ChooseClassesViewModel ViewModel
    {
        get;
    }

    public ChooseClassesPage()
    {
        ViewModel = App.GetService<ChooseClassesViewModel>();
        InitializeComponent();
    }

    private void reloadInformations(string periodId, string courseId, string courseName, string status)
    {
        ViewModel.LoadInformations(periodId, courseId, courseName, status);

        if (ViewModel.Informations.RegisteredClassId != null)
        {
            ClassDataGrid.SelectedIndex = ViewModel.Informations.Classes.FindIndex(c => c.Id == ViewModel.Informations.RegisteredClassId);
        }

        if (ViewModel.Informations.Status == resourceLoader.GetString("ChooseCourses_RegisteredStatus"))
        {
            Cancel_Button.Visibility = Visibility.Visible;
        }
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var periodInfo = e.Parameter as Tuple<string, string, string, string>;
        await Task.Delay(10);

        reloadInformations(periodInfo.Item1, periodInfo.Item2, periodInfo.Item3, periodInfo.Item4);

        ClassesListLoading.Visibility = Visibility.Collapsed;
        ClassesList.Visibility = Visibility.Visible;
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
        if ((e.Record as ClassOfCourse)?.Id != ViewModel.Informations.RegisteredClassId)
        {
            registerClass();
        }
    }

    private async void Refresh_Click(object sender, RoutedEventArgs e)
    {
        ClassesListLoadingRing.Visibility = Visibility.Visible;
        ClassesList.Opacity = 0.5;
        await Task.Delay(10);

        reloadInformations(ViewModel.Informations.PeriodId, ViewModel.Informations.CourseId, 
            ViewModel.Informations.CourseName, ViewModel.Informations.Status);

        ClassesListLoadingRing.Visibility = Visibility.Collapsed;
        ClassesList.Opacity = 1;
    }

    private async void Register_Button_Click(object sender, RoutedEventArgs e)
    {
        if (ClassDataGrid.SelectedIndex < 0)
        {
            return;
        }

        if ((ClassDataGrid.SelectedItem as ClassOfCourse)?.Id == ViewModel.Informations.RegisteredClassId)
        {
            return;
        }

        if (ViewModel.Informations.Status == resourceLoader.GetString("ChooseCourses_StudiedStatus"))
        {
            var dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Title = resourceLoader.GetString("ChooseClasses_ConfirmTitle"),
                Content = resourceLoader.GetString("ChooseClasses_DeleteOldResultAlert"),
                PrimaryButtonText = resourceLoader.GetString("ChooseClasses_ContinueDialogButton"),
                PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                CloseButtonText = resourceLoader.GetString("App_Close/Text"),
                RequestedTheme = App.GetService<IThemeSelectorService>().Theme
            };
            dialog.PrimaryButtonClick += (s, e) => { registerClass(); };

            await dialog.ShowAsync();
        }
        else
        {
            registerClass();
        }
    }

    private async void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = resourceLoader.GetString("ChooseClasses_ConfirmTitle"),
            Content = resourceLoader.GetString("ChooseClasses_CancelCourseConfirmMessage"),
            PrimaryButtonText = resourceLoader.GetString("ChooseClasses_ContinueDialogButton"),
            PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };
        dialog.PrimaryButtonClick += (s, e) => { cancelCourse(); }; 

        await dialog.ShowAsync();
    }

    private void showNotifications(string title, string message)
    {
        var toastBuilder = new AppNotificationBuilder()
            .AddText(title)
            .AddText(message);

        var toast = toastBuilder.BuildNotification();
        AppNotificationManager.Default.Show(toast);
    }

    private async void cancelCourse()
    {
        ContentArea.Opacity = 0.5;
        PageLoadingOverlay.Visibility = Visibility.Visible;
        await Task.Delay(10);

        if (ViewModel.CancelCourse(ViewModel.Informations.RegisteredClassId) == "OK")
        {
            showNotifications(resourceLoader.GetString("ChooseClasses_CancelCourseTitleToast"),
                    resourceLoader.GetString("ChooseClasses_CancelCourseMessageToast"));

            var navigationService = App.GetService<INavigationService>();
            navigationService.GoBack();
        }
        else
        {
            ContentArea.Opacity = 1;
            PageLoadingOverlay.Visibility = Visibility.Collapsed;
            showNotifications(resourceLoader.GetString("ChooseClasses_CancelCourseTitleToast"),
                        resourceLoader.GetString("ChooseClasses_CancelCourseFailMessageToast"));
        }
    }

    private async void registerClass()
    {
        ContentArea.Opacity = 0.5;
        PageLoadingOverlay.Visibility = Visibility.Visible;
        await Task.Delay(10);

        var code = ViewModel.RegisterClass(ViewModel.Informations.Classes[ClassDataGrid.SelectedIndex].Id, ViewModel.Informations.RegisteredClassId);
        var dialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = resourceLoader.GetString("ChooseClasses_RegisterNotificationTitle"),
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            RequestedTheme = App.GetService<IThemeSelectorService>().Theme
        };
        
        ContentArea.Opacity = 1;
        PageLoadingOverlay.Visibility = Visibility.Collapsed;

        var displayDialog = async (string message, string status) =>
        {
            showNotifications(resourceLoader.GetString("ChooseClasses_RegisterNotificationTitle"), message);

            dialog.Content = message;
            dialog.CloseButtonClick += (s, e) =>
            {
                reloadInformations(ViewModel.Informations.PeriodId, ViewModel.Informations.CourseId,
                    ViewModel.Informations.CourseName, status);
            };
            await dialog.ShowAsync();
        };

        if (code == "OK")
        {
            await displayDialog(resourceLoader.GetString("ChooseClasses_RegisterClassSuccessMessageDialog"),
                resourceLoader.GetString("ChooseClasses_RegisteredSlot/HeaderText"));
        }
        else if (code == "DUP")
        {
            await displayDialog(resourceLoader.GetString("ChooseClasses_RegisterClassTimeDuplicateMessageDialog"),
                ViewModel.Informations.Status);
        }
        else if (code == "NOSLOT")
        {
            await displayDialog(resourceLoader.GetString("ChooseClasses_RegisterClassNoSlotMessageDialog"),
                ViewModel.Informations.Status);
        }
        else
        {
            await displayDialog(resourceLoader.GetString("ChooseClasses_RegisterClassFailMessageDialog"),
                ViewModel.Informations.Status);
        }
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }
}



