using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SpacePortal.Helpers;
public class CurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var currentLangue = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
        CultureInfo culture = new CultureInfo(currentLangue);
        return string.Format(culture, "{0:N2}", value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
