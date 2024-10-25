using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;

namespace SpacePortal.Models;
public class InformationsForShellPage : INotifyPropertyChanged
{
    public string AvatarUrl
    {
        get; set;
    }
    public BitmapImage AvatarBitmap
    {
        get; set;
    }

    public string StudentName
    {
        get; set;
    }

    public string StudentSchoolEmail
    {
        get; set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetAvatarBitmap(string filePath)
    {
        AvatarBitmap = new BitmapImage(new Uri(filePath));
    }
}
