using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models
{
    public class InformationsForInformation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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
        public string AvatarUrl
        {
            get; set;
        }

    }
}
