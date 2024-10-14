using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class RequestPage : Page
{
    public RequestViewModel ViewModel
    {
        get;
    }

    public RequestPage()
    {
        ViewModel = App.GetService<RequestViewModel>();
        InitializeComponent();
    }
}
