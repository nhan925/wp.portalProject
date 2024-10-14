using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class NotificationPage : Page
{
    public NotificationViewModel ViewModel
    {
        get;
    }

    public NotificationPage()
    {
        ViewModel = App.GetService<NotificationViewModel>();
        InitializeComponent();
    }
}
