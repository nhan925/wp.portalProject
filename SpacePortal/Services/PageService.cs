﻿using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SpacePortal.Contracts.Services;
using SpacePortal.ViewModels;
using SpacePortal.Views;

namespace SpacePortal.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = new();

    public PageService()
    {
        Configure<DashboardViewModel, DashboardPage>();
        Configure<InformationViewModel, InformationPage>();
        Configure<NotificationViewModel, NotificationPage>();
        Configure<GradesViewModel, GradesPage>();
        Configure<ScheduleViewModel, SchedulePage>();
        Configure<CoursesRegistrationViewModel, CoursesRegistrationPage>();
        Configure<TuitionFeeViewModel, TuitionFeePage>();
        Configure<PaymentHistoryViewModel, PaymentHistoryPage>();
        Configure<ScholarshipViewModel, ScholarshipPage>();
        Configure<RequestViewModel, RequestPage>();
        Configure<AIChatbotViewModel, AIChatbotPage>();
        Configure<AppFeedbackViewModel, AppFeedbackPage>();
        Configure<SettingsViewModel, SettingsPage>();
        Configure<RequestDetailViewModel, RequestDetailPage>();
        Configure<ChooseCoursesViewModel, ChooseCoursesPage>();
        Configure<ChooseClassesViewModel, ChooseClassesPage>();
        Configure<TuitionFeeDetailViewModel, TuitionFeeDetailPage>();
        Configure<CourseFeedbackViewModel, CourseFeedbackPage>();
        Configure<CourseFeedbackDetailViewModel, CourseFeedbackDetailPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (_pages)
        {
            var key = typeof(VM).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
