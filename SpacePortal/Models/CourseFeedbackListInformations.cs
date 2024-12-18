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

        public bool status
        {
            get; set;
        }

        public bool StautsForClick
        {
            get
            {
                return !status;
            }
        }

        public KeyValuePair<string, string> StatusWithItsColor
        {
            get
            {
                ResourceLoader resourceLoader = new();

                if (status)
                {
                    return new(resourceLoader.GetString("CourseFeedback_Rated/Text"), "LimeGreen");
                }
                else
                {
                    return new(resourceLoader.GetString("CourseFeedback_NotRated/Text"), "Red");
                }

            }
        }
    }

}
