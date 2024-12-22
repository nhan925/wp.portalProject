using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.UI.Xaml.Data;

namespace SpacePortal.Helpers;
public class IntToCurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int intValue)
        {
            // Định dạng số thành tiền tệ VNĐ
            return string.Format(new CultureInfo("vi-VN"), "{0:N0} VNĐ", intValue);
        }
        throw new ArgumentException("Value must be an integer.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException(); // Không cần thiết cho trường hợp này
    }
}
