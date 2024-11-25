using DotNetEnv;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.System;

using SpacePortal.Activation;
using SpacePortal.Contracts.Services;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Contracts.Services;
using SpacePortal.Core.Models;
using SpacePortal.Core.Services;
using SpacePortal.DataAccess;
using SpacePortal.Helpers;
using SpacePortal.Models;
using SpacePortal.Services;
using SpacePortal.ViewModels;
using SpacePortal.Views;

using Syncfusion.Licensing;
using WinUIEx.Messaging;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using WinUIEx;
using Microsoft.UI.Xaml.Media;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.UI.Xaml.Documents;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Imaging;
namespace SpacePortal;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; set; } = new MainWindow();

    public static Window LoginWindow { get; set; } = new LoginWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        // Load .env file
        var envFilePath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envFilePath);

        // Syncfusion License
        var syncfusionLicenseKey = Env.GetString("SYNCFUSION");
        SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

        // Get API information
        var apiUrl = Env.GetString("API_URL");

        // Get Imgur client ID
        var imgurClientId = Env.GetString("IMGUR_CLIENT_ID");

        // TODO: Set the default language here
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "vi-VN";

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();
            services.AddSingleton<ApiService>(provider => new ApiService(apiUrl));
            services.AddSingleton<ImgurService>(provider => new ImgurService(imgurClientId));

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ILocalSettingsService,  LocalSettingsService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<ChooseClassesViewModel>();
            services.AddTransient<ChooseClassesPage>();
            services.AddTransient<ChooseCoursesViewModel>();
            services.AddTransient<ChooseCoursesPage>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<AppFeedbackViewModel>();
            services.AddTransient<AppFeedbackPage>();
            services.AddTransient<AIChatbotViewModel>();
            services.AddTransient<AIChatbotPage>();
            services.AddTransient<RequestViewModel>();
            services.AddTransient<RequestPage>();
            services.AddTransient<ScholarshipViewModel>();
            services.AddTransient<ScholarshipPage>();
            services.AddTransient<PaymentHistoryViewModel>();
            services.AddTransient<PaymentHistoryPage>();
            services.AddTransient<TuitionFeeViewModel>();
            services.AddTransient<TuitionFeePage>();
            services.AddTransient<CoursesRegistrationViewModel>();
            services.AddTransient<CoursesRegistrationPage>();
            services.AddTransient<ScheduleViewModel>();
            services.AddTransient<SchedulePage>();
            services.AddTransient<GradesViewModel>();
            services.AddTransient<GradesPage>();
            services.AddTransient<NotificationViewModel>();
            services.AddTransient<NotificationPage>();
            services.AddTransient<InformationViewModel>();
            services.AddTransient<InformationPage>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<RequestPhysicalTranscriptDialog>();
            services.AddTransient<RequestPhysicalTranscriptDialogViewModel>();
            services.AddTransient<EstimateAverageGradeDialog>();
            services.AddTransient<EstimateAverageGradeDialogViewModel>();
            services.AddTransient<RequestReExaminationDialog>();
            services.AddTransient<RequestReExaminationDialogViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.AddSingleton<IDao<InformationsForInformation>, InformationsForInformationDao>();
            services.AddSingleton<IDao<InformationsForDashboard>, InformationsForDashboardDao>();
            services.AddSingleton<IDao<InformationsForShellPage>, InformationsForShellPageDao>();
            services.AddSingleton<IDao<InformationsForGradesPage_GradesRow>,InformationsForGradesPageDao>();
            services.AddSingleton<IDao<CoursesRegistrationPeriodInformation>, CoursesRegistrationPeriodInformationDao>();
            services.AddSingleton<IDao<ChooseCoursesInformations>, ChooseCoursesInformationsDao>();
            services.AddSingleton<IDao<ChooseClassesInformations>, ChooseClassesInformationsDao>();
        }).
        Build();
         
        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var messageTextBlock = new TextBlock
        {
            Text = $"{(new ResourceLoader()).GetString("App_CrashMessage")}\n{e.Exception.Message}",
            TextWrapping = TextWrapping.WrapWholeWords,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontSize = 14,
        };

        var container = new StackPanel() { Orientation = Orientation.Horizontal, Spacing=24, 
            HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        container.Children.Add(new FontIcon() { Glyph = "\uEB90", Foreground = new SolidColorBrush(Colors.Red), FontSize = 28 });
        container.Children.Add(messageTextBlock);

        container.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));

        var width = container.DesiredSize.Width + 64;
        var height = container.DesiredSize.Height + 96;

        var errorWindow = new WindowEx
        {
            Title = "Crashed",
            IsAlwaysOnTop = true,
            IsMaximizable = false,
            IsMinimizable = false,
            IsResizable = false,
            Content = container,
            Width = width,
            Height = height,
            SystemBackdrop = new MicaBackdrop(),
        };
        errorWindow.CenterOnScreen();
        errorWindow.Closed += (s, e) => { App.Current.Exit(); };
        errorWindow.Activate();

        MainWindow.Close();
        LoginWindow.Close();

        e.Handled = true;
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        LoginWindow = new LoginWindow(args);
        LoginWindow.Activate();

        //await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
