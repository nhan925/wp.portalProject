using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Models;
public class CoursesRegistrationPeriodInformation : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int Id
    {
        get; set;
    }

    public string Title
    {
        get; set;
    }

    public DateTime OpenDate
    {
        get; set;
    }

    public DateTime CloseDate
    {
        get; set;
    }

    public bool CanRegister => OpenDate <= DateTime.Now && DateTime.Now <= CloseDate;

    public KeyValuePair<string, string> StatusWithItsColor
    {
        get
        {
            ResourceLoader resourceLoader = new();
            
            if (OpenDate > DateTime.Now)
            {
                return new (resourceLoader.GetString("CoursesRegistration_PeriodStatusNotOpened"), "Gray");
            }
            else if (CloseDate < DateTime.Now)
            {
                return new(resourceLoader.GetString("CoursesRegistration_PeriodStatusClosed"), "Red");
            }
            else
            {
                return new(resourceLoader.GetString("CoursesRegistration_PeriodStatusOpening"), "LimeGreen");
            }
        }
    }
}
