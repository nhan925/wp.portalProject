using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class TuitionFeePage : Page
{
    public TuitionFeeViewModel ViewModel
    {
        get;
    }

    public TuitionFeePage()
    {
        ViewModel = App.GetService<TuitionFeeViewModel>();
        InitializeComponent();
    }
}
