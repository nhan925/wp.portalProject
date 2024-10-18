using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace SpacePortal.Helpers;

public class EnumToStringConverter : IValueConverter
{
    public EnumToStringConverter()
    {
    }

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Check if value is a valid enum
        if (value == null || !Enum.IsDefined(value.GetType(), value))
        {
            throw new ArgumentException("EnumToStringConverter value must be a valid enum.");
        }

        // Return the enum value as a string
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
