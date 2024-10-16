using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
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

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        // Load .env file
        var envFilePath = Path.Combine(AppContext.BaseDirectory, ".env");
        Env.Load(envFilePath);
        var syncfusionLicenseKey = Env.GetString("SYNCFUSION");
        SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);

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

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
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

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
            services.AddSingleton<IDao<InformationsForDashboard>, InformationsForDashboardDao>();
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
