using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.ViewModels;
using Windows.ApplicationModel.VoiceCommands;

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
        ViewModel.Init();
    }

    private void ShowGradeButton_Click(object sender, RoutedEventArgs e)
    {
        var cbYear = ComboBoxYear.SelectedItem?.ToString();
        var cbSemester = ComboBoxSemester.SelectedItem?.ToString();

        if (cbYear != null && cbSemester != null)
        {
            ViewModel.ShowGradeByYearAndSemester(cbYear,cbSemester);
        }
        else if (cbYear != null)
        {
            ViewModel.ShowGradeByYear(cbYear);
        }
        else if (cbSemester != null)
        {
            ViewModel.ShowGradeBySemester(cbSemester);
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
