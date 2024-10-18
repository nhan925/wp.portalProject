using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;


namespace SpacePortal.DataAccess;
public class InformationsForInformationDao : IDao<InformationsForInformation>
{
    public void Add(InformationsForInformation entity)
    {

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public ObservableCollection<InformationsForInformation> GetAll()
    {
        throw new NotImplementedException();
    }

    public InformationsForInformation GetById(int id)
    {
        return new InformationsForInformation()
        {
            FullName = "Nguyễn Minh Nguyệt",
            Major = "Công nghệ thông tin",
            AcademicProgram = "Đại trà",
            StudentID = "22120238",
            YearOfAdmission = 2022,
            Gender = "Nam",
            DateOfBirth = new DateTime(2004, 10, 10),
            IdentityCardNumber = "082204000661",
            Nationality = "Việt Nam",
            Ethnicity = "Kinh",
            SchoolEmail = "22120238@student.hcmus.edu.vn",
            PersonalEmail = "nguyenptn1104@gmail.com",
            PhoneNumber = "0987654321",
            Address = "123 Đường Nguyễn Trãi, Quận 1, TP.HCM",
            AvatarUrl = new BitmapImage(new Uri("ms-appx:///Assets/defaultAvt.png"))

        };

        //throw new NotImplementedException();
    }


    public void Update(InformationsForInformation entity)
    {
        throw new NotImplementedException();
    }
}
