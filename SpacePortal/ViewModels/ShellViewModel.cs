using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Navigation;

using SpacePortal.Contracts.Services;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using SpacePortal.Views;

namespace SpacePortal.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    public InformationsForShellPage Informations
    {
        get;
    }

    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    public INavigationService NavigationService
    {
        get;
    }

    public INavigationViewService NavigationViewService
    {
        get;
    }

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;

        Informations = App.GetService<IDao<InformationsForShellPage>>().GetById(null);
        if (String.IsNullOrEmpty(Informations.AvatarUrl))
        {
            Informations.SetAvatarBitmap("ms-appx:///Assets/defaultAvt.png");
        }
        else
        {
            Informations.SetAvatarBitmap(Informations.AvatarUrl);
        }
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    public string ChangePassword(string currentPassword, string newPassword)
    {
        var payload = new
        {
            current_password = currentPassword,
            new_password = newPassword,
        };

        return App.GetService<ApiService>().Post<string>("/rpc/change_password", payload);
    }
}
