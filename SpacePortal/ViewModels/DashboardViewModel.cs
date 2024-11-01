using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;
using Windows.ApplicationModel.Resources;
using Windows.Management.Deployment.Preview;
using Windows.Networking.Sockets;
using Windows.System.UserProfile;

namespace SpacePortal.ViewModels;

public partial class DashboardViewModel : ObservableRecipient
{
    public string TodayDate
    {
        get
        {
            var currentLanguage = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
            CultureInfo culture = new CultureInfo(currentLanguage);
            return DateTime.Now.ToString("D", culture);
        }
    }

    public InformationsForDashboard Informations
    {
        get;
    }

    public DashboardViewModel()
    {
        Informations = App.GetService<IDao<InformationsForDashboard>>().GetById(null);
    }
}
