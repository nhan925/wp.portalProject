using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class InformationPage : Page
{
    public InformationViewModel ViewModel
    {
        get;
    }

    public InformationPage()
    {
        ViewModel = App.GetService<InformationViewModel>();
        InitializeComponent();
    }

    private void EditAvatar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ///TODO
    }

    private void RemoveAvatar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //TODO
    }

    private void ChangeAvatar_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //TODO

    }

    private void ChangeContact_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        //TODO
    }
}
