using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.DataAccess;
using Microsoft.Windows.ApplicationModel.Resources;
namespace SpacePortal.Models;
public class TuitionFeeListInformations
{

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

    public bool status
    {
        get;
        set;
    }   

    public bool StautsForClick
    {
        get
        {
            return !status;
        }
    }

    public string Title
    {
        get
        {
            ResourceLoader resourceLoader = new();
            return resourceLoader.GetString("TuitionFee_Year/Text") + " " + year + " | " + resourceLoader.GetString("TuitionFee_Semester/Text") + " " + semester.ToString();
        }
    }

    public KeyValuePair<string, string> StatusWithItsColor
    {
        get
        {
            ResourceLoader resourceLoader = new();

            if (status)
            {
                return new(resourceLoader.GetString("TuitionFee_Paid/Text"), "LimeGreen");
            }
            else
            {
                return new(resourceLoader.GetString("TuitionFee_NotPaid/Text"), "Red");
            }
         
        }
    }

}
