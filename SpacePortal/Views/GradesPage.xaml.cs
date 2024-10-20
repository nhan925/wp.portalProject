﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Windows.ApplicationModel.VoiceCommands;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Views;

public sealed partial class GradesPage : Page
{
    private readonly ResourceLoader resourceLoader = new();

    private readonly ResourceLoader resourceLoader = new();

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

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {

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
}
