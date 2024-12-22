using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;
namespace SpacePortal.Models
{
    public class CourseFeedbackListInformations
    {
        public string courseName
        {
            get; set;
        }

        public string courseID
        {
            get; set;
        }

        public string teacherName
        {
            get; set;
        }

        public string classID
        {
            get; set;
        }

        public string status
        {
            get; set;
        }

        public bool StautsForClick
        {
            get
            {
                if (status == "NOT_FEEDBACK")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public KeyValuePair<string, string> StatusWithItsColor
        {
            get
            {
                ResourceLoader resourceLoader = new();

                if (status == "DONE")
                {
                    return new(resourceLoader.GetString("CourseFeedback_Rated/Text"), "LimeGreen");
                }
                else if (status == "NOT_YET")
                {
                    return new(resourceLoader.GetString("CourseFeedback_NotYet/Text"), "Gray");
                }
                else
                {
                    return new(resourceLoader.GetString("CourseFeedback_NotRated/Text"), "Red");
                }

            }
        }
    }

}
