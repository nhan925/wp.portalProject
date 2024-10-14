using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Core.Models;
internal class Student : INotifyPropertyChanged
{
    public string StudentId
    {
        get; set;
    }
    public string IdentityCardNumber
    {
        get; set;
    }
    public string FullName
    {
        get; set;
    }
    public string Address
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
    public string PhoneNumber
    {
        get; set;
    }
    public string AcademicProgram
    {
        get; set;
    }
    public string PersonalEmail
    {
        get; set;
    }
    public string SchoolEmail
    {
        get; set;
    }
    public string AvatarUrl
    {
        get; set;
    }
    public int YearOfAdmission
    {
        get; set;
    }
    public int EarnedCredits
    {
        get; set;
    }
    public decimal CGPA
    {
        get; set;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
