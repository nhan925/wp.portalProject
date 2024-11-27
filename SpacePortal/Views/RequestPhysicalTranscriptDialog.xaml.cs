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
public sealed partial class RequestPhysicalTranscriptDialog : Page
{
    public RequestPhysicalTranscriptDialogViewModel ViewModel
    {
        get;
    }

    public RequestPhysicalTranscriptDialog(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData)
    {
        this.InitializeComponent();
        ViewModel = new(sourceData);
    }

    private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (double.IsNaN(sender.Value))
        {
            sender.Value = 0;
        }
    }

    public void PrimaryButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs e)
    {
        var selectedSemester = SemesterComboBox.SelectedItem?.ToString() ?? string.Empty;
        var selectedYear = YearComboBox.SelectedItem?.ToString() ?? string.Empty;
        if (ViewModel.TotalTranscripts == 0)
        {
            e.Cancel = true;
        }
        else
        {
            ViewModel.SendRequestForTranscripts(selectedSemester,selectedYear);
        }
    }
}
