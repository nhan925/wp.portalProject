using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;

namespace SpacePortal.Models;
public class PaymentHistoryInfomations
{
    private ResourceLoader resourceLoader = new();
    public int Id
    {
        get; set;
    }
    public double Amount
    {
        get; set;
    }
    public DateTime Date
    {
        get; set;
    }
    
    public string Type { get; set; } = string.Empty; // Default to an empty string
    
    public bool IsSuccess
    {
        get; set;
    }

    public string Status
    {
        get
        {
            if (IsSuccess)
            {
                return resourceLoader.GetString("PaymentHistory_StatusSucess");
            }
            else
            {
                return resourceLoader.GetString("PaymentHistory_StatusCancelled");
            }
        }
    }

    public string TypeString
    {
        get
        {
            if (Type == "TUITION")
            {
                return resourceLoader.GetString("PaymentHistory_TypeTuitionFee");
            }
            else
            {
                return resourceLoader.GetString("PaymentHistory_TypeGradeReview");
            }
        }
    }
}
