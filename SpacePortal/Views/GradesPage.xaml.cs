using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

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

    private void ToggleThemeTeachingTip2_ActionButtonClick(global::Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
    {
        // Your event handling code here
    }
}
