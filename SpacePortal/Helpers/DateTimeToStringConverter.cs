using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SpacePortal.Helpers;
public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var currentLangue = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;

        CultureInfo culture = new CultureInfo(currentLangue);

        string result = "";
        result = ((DateTime)value).ToString("dd/MM/yyyy", culture);
        return result;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
