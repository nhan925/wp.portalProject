using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class CoursesRegistrationPage : Page
{
    public CoursesRegistrationViewModel ViewModel
    {
        get;
    }

    public CoursesRegistrationPage()
    {
        ViewModel = App.GetService<CoursesRegistrationViewModel>();
        InitializeComponent();
    }
}
