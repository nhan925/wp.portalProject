using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models
{
    public class TuitionFeeDetailCourse: INotifyPropertyChanged
    {
        public string courseName
        {
            get; set;
        }

        public string courseID
        {
            get; set;
        }

        public int courseFee
        {
            get; set;
        }

        public int courseCredit
        {
            get; set;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
