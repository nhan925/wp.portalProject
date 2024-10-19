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
using SpacePortal.Core.Models;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SpacePortal.Views;

public sealed partial class EstimateAverageGradeDialog : Page
{
    EstimateAverageGradeDialogViewModel ViewModel
    {
        get;
    }

    public EstimateAverageGradeDialog(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData,
        InformationsForEstimateAverageGradeDialog degreeAndCreditInfo)
    {
        this.InitializeComponent();
        ViewModel = new(sourceData, degreeAndCreditInfo);
    }

    private void DegreeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedDegreeType = (string)DegreeTypeComboBox.SelectedItem;
        ViewModel.CalculateEstimatedGrade(selectedDegreeType);
    }
}
