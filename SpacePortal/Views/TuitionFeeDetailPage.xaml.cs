using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using SpacePortal.Contracts.Services;
using SpacePortal.Models;
using SpacePortal.ViewModels;
using Syncfusion.UI.Xaml.DataGrid;
using Windows.ApplicationModel.Resources;
using ResourceLoader = Windows.ApplicationModel.Resources.ResourceLoader;

namespace SpacePortal.Views;

public sealed partial class TuitionFeeDetailPage : Page
{
    ResourceLoader resourceLoader = new ResourceLoader();

    public TuitionFeeDetailViewModel ViewModel
    {
        get;
    }

    public TuitionFeeDetailPage()
    {
        ViewModel = App.GetService<TuitionFeeDetailViewModel>();
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await Task.Delay(10);
        if (e.Parameter is Tuple<int, string, int, int, int> info)
        {
            ViewModel.LoadInformations(info.Item1, info.Item2, info.Item3, info.Item4, info.Item5);
        }

        CourseFeeDataGrid.Visibility = Visibility.Visible;
        CourseFeeListLoading.Visibility = Visibility.Collapsed;
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        var navigationService = App.GetService<INavigationService>();
        navigationService.GoBack();
    }

    private void sfDataGrid_QueryRowHeight(object sender, Syncfusion.UI.Xaml.DataGrid.QueryRowHeightEventArgs e)
    {
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        var autoHeight = double.NaN;
        var datagrid = sender as SfDataGrid;
        if (datagrid.ColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out autoHeight))
        {
            e.Height = Math.Max(autoHeight + 8, datagrid.RowHeight + 8);
            e.Handled = true;
        }
    }


    private DispatcherTimer _timer;
    private Stopwatch _stopwatch;
    private readonly TimeSpan _timeout = TimeSpan.FromMinutes(5);
    private bool _isPaymentSuccess = false;
    private WindowEx paymentWindow;


    private async void Payment_Click(object sender, RoutedEventArgs e)
    {

        var order_url = ViewModel.CallApiToPayment();

        var webView2 = new WebView2();
        await webView2.EnsureCoreWebView2Async();
        webView2.Source = new Uri(order_url);

        webView2.CoreWebView2.ContextMenuRequested += (sender, e) =>
        {
            e.Handled = true;
        };

        paymentWindow = new()
        {
            Title = resourceLoader.GetString("TuitionFeeDetail_WindowPaymentTitle/Text"),
            MinWidth = 1100,
            MinHeight = 600,
            SystemBackdrop = new MicaBackdrop(),
            IsMaximizable = false,
            IsResizable = false,
        };
        
        paymentWindow.Content = webView2;
        paymentWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        paymentWindow.CenterOnScreen();
        paymentWindow.Closed += (sender, e) =>
        {
            if (!_isPaymentSuccess)
            {
                ShowPaymentFailedMessage();

            }
            else
            {
                ShowPaymentSuccessMessage();
            }
            App.MainWindow.Show();
        };
        paymentWindow.Activate();
        App.MainWindow.Hide();



        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(5);
        _timer.Tick += Timer_Tick;

        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        _timer.Start();

    }


    private async void Timer_Tick(object sender, object e)
    {

        var paymentSuccess = await ViewModel.CheckPaymentStatus();

        if (paymentSuccess)
        {
            _timer.Stop();
            _stopwatch.Stop();
            _isPaymentSuccess = true;
            paymentWindow.Close();
          
        }
    }

    private void ShowPaymentSuccessMessage()
    {
       
        var dialog = new ContentDialog
        {
            Title = resourceLoader.GetString("TuitionFeeDetail_PaymentDialog_SuccessTitle/Text"),
            Content = resourceLoader.GetString("TuitionFeeDetail_PaymentDialog_SuccessContent/Text"),
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            XamlRoot = this.XamlRoot
        };

        dialog.CloseButtonClick += async (sender, args) =>
        {
            ViewModel.AddPaymentHistoryToDatabase(true);
            var navigationService = App.GetService<INavigationService>();
            navigationService.GoBack();
        };

        _ = dialog.ShowAsync();
    }

    private void ShowPaymentFailedMessage()
    {
        var dialog = new ContentDialog
        {
            Title = resourceLoader.GetString("TuitionFeeDetail_PaymentDialog_FailedTitle/Text"),
            Content = resourceLoader.GetString("TuitionFeeDetail_PaymentDialog_FailedContent/Text"),
            CloseButtonText = resourceLoader.GetString("App_Close/Text"),
            XamlRoot = this.XamlRoot
        };

        ViewModel.AddPaymentHistoryToDatabase(false);
        _ = dialog.ShowAsync();
    }


}
