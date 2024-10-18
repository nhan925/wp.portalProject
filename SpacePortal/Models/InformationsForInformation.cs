using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;

namespace SpacePortal.Models;

public class InformationsForInformation : INotifyPropertyChanged
{

    public string FullName
    {
        get; set;
    }
    public string Major
    {
        get; set;
    }
    public string AcademicProgram
    {
        get; set;
    }
    public string StudentID
    {
        get; set;
    }
    public int YearOfAdmission
    {
        get; set;
    }
    public string Gender
    {
        get; set;
    }
    public DateTime DateOfBirth
    {
        get; set;
    }
    public string IdentityCardNumber
    {
        get; set;
    }
    public string Nationality
    {
        get; set;
    }
    public string Ethnicity
    {
        get; set;
    }
    public string SchoolEmail
    {
        get; set;
    }
    public string Address
    {
        get; set;
    }
    public string PhoneNumber
    {
        get; set;
    }
    public string PersonalEmail
    {
        get; set;
    }
    public BitmapImage AvatarUrl
    {
        get; set;
    }

    public void SetAvatarUrl(string filePath)
    {
        AvatarUrl = new BitmapImage(new Uri(filePath));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
