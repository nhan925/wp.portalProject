using Microsoft.UI.Xaml.Controls;

using SpacePortal.ViewModels;

namespace SpacePortal.Views;

public sealed partial class PaymentHistoryPage : Page
{
    public PaymentHistoryViewModel ViewModel
    {
        get;
    }

    public PaymentHistoryPage()
    {
        ViewModel = App.GetService<PaymentHistoryViewModel>();
        InitializeComponent();
    }
}
