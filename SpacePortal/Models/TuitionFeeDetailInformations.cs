using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
namespace SpacePortal.Models;
public class TuitionFeeDetailInformations: INotifyPropertyChanged
{
    public int semesterID
    {
        get; set;
    }  

    public string year
    {
        get; set;
    }

    public int semester
    {
        get; set;
    }

    public int totalCourse
    {
        get; set;
    }

    public int totalTuitionFee
    {
        get; set;
    }

    public string Title
    {
        get
        {
            ResourceLoader resourceLoader = new();
            return resourceLoader.GetString("TuitionFee_Year/Text") + " " + year + " | " + resourceLoader.GetString("TuitionFee_Semester/Text") + " " + semester.ToString();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
