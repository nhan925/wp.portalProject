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

namespace SpacePortal.Views;

public sealed partial class GradesPage : Page
{

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
}
