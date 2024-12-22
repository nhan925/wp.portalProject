using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SpacePortal.Helpers;
public class RatingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int rating && int.TryParse(parameter as string, out int radioValue))
        {
            return rating == radioValue;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isChecked && isChecked && int.TryParse(parameter as string, out int radioValue))
        {
            return radioValue;
        }
        return 0;
    }
}
