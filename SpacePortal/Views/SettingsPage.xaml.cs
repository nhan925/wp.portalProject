using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.Helpers;
using SpacePortal.ViewModels;

namespace SpacePortal.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    private readonly ResourceLoader resourceLoader = new();

    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ElementTheme selectedTheme;
        if (LightItem.IsSelected)
        {
            selectedTheme = ElementTheme.Light;
        }
        else if (DarkItem.IsSelected)
        {
            selectedTheme = ElementTheme.Dark;
        }
        else
        {
            selectedTheme = ElementTheme.Default;
        }

        ViewModel.SwitchThemeCommand.Execute(selectedTheme);
    }

    private void ThemeComboBox_Loaded(object sender, RoutedEventArgs e)
    {
        var selectedIndex = 2;
        var selectedTheme = ViewModel.ElementTheme;
        if (selectedTheme == ElementTheme.Light)
        {
            selectedIndex = 0;
        }
        else if (selectedTheme == ElementTheme.Dark)
        {
            selectedIndex = 1;
        }
        ThemeComboBox.SelectedIndex = selectedIndex;
    }

    private void ThemeSettingCard_Loaded(object sender, RoutedEventArgs e)
    {

        ThemeSettingCard.Header = resourceLoader.GetString("Settings_ThemeHeader");
        ThemeSettingCard.Description = resourceLoader.GetString("Settings_ThemeDescription");
    }

    private void AboutSettingExpander_Loaded(object sender, RoutedEventArgs e)
    {
        aboutSettingExpander.Header = resourceLoader.GetString("AppDisplayName");
        aboutSettingExpander.Description = resourceLoader.GetString("Settings_AboutLicense");
    }

    private async void GithubOpenButton_Click(object sender, RoutedEventArgs e)
    {
        var githubUrl = resourceLoader.GetString("Settings_GithubUrl");
        await Windows.System.Launcher.LaunchUriAsync(new Uri(githubUrl));
    }

    private void GithubOpenButton_Loaded(object sender, RoutedEventArgs e)
    {
        GithubOpenButton.Header = resourceLoader.GetString("Settings_GithubHeader");
    }

    private void FeedbackButton_Click(object sender, RoutedEventArgs e)
    {
        App.GetService<INavigationService>().NavigateTo(typeof(AppFeedbackViewModel).FullName
            ?? "SpacePortal.ViewModels.AppFeedbackViewModel");
    }
}
