using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.DataAccess;
using Microsoft.Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources;
namespace SpacePortal.Models;
public class TuitionFeeListInformations
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

    public string status
    {
        get;
        set;
    }

    public bool StautsForClick
    {
        get
        {
            if (status == "NOT_PAID")
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public string Title
    {
        get
        {
            Windows.ApplicationModel.Resources.ResourceLoader resourceLoader = new();
            return resourceLoader.GetString("TuitionFee_Year/Text") + " " + year + " | " + resourceLoader.GetString("TuitionFee_Semester/Text") + " " + semester.ToString();
        }
    }

    public KeyValuePair<string, string> StatusWithItsColor
    {
        get
        {
            Windows.ApplicationModel.Resources.ResourceLoader resourceLoader = new();

            if (status == "PAID")
            {
                return new(resourceLoader.GetString("TuitionFee_Paid/Text"), "LimeGreen");
            }
            else if (status == "NOT_PAID")
            {
                return new(resourceLoader.GetString("TuitionFee_NotPaid/Text"), "Red");
            }
            else if (status == "NOT_YET")
            {
                return new(resourceLoader.GetString("TuitionFee_NotYet/Text"), "Gray");
            }
            else
            {
                return new(resourceLoader.GetString("TuitionFee_Overdue/Text"), "Orange");
            }

        }
    }

}
