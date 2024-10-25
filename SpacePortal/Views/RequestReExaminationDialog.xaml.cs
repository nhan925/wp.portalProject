using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpacePortal.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RequestReExaminationDialog : Page
{
   
    public RequestReExaminationDialogViewModel ViewModel
    {
        get;
    }

    public RequestReExaminationDialog(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData)
    {
        this.InitializeComponent();
        ViewModel = new RequestReExaminationDialogViewModel(sourceData);
    }

    public void SendRequest(ContentDialog sender, ContentDialogButtonClickEventArgs e)
    {
        if (ViewModel.SelectedYear == null || ViewModel.SelectedSemester == null || ViewModel.SelectedSubject == null)
        {
            e.Cancel = true;
        }
        else
        {
            ViewModel.SendRequest();
        }
    }

    private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (YearComboBox.SelectedItem == null)
        {
            SemesterComboBox.IsEnabled = false;
            ViewModel.SelectedSemester = null;
        }
        else
        {
            SemesterComboBox.IsEnabled = true;
        }

        if (YearComboBox.SelectedItem != null)
        {
            ViewModel.SelectedSemester = null;
        }
    }
}
