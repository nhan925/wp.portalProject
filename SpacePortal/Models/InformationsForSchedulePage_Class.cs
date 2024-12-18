using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;

namespace SpacePortal.Models;
public class InformationsForSchedulePage_Class : INotifyPropertyChanged
{
    private DateTime _from;

    private DateTime _to;

    Dictionary<string, DayOfWeek> vietnameseDays = new Dictionary<string, DayOfWeek>
    {
        {"Chủ Nhật", DayOfWeek.Sunday},
        {"Thứ 2", DayOfWeek.Monday},
        {"Thứ 3", DayOfWeek.Tuesday},
        {"Thứ 4", DayOfWeek.Wednesday},
        {"Thứ 5", DayOfWeek.Thursday},
        {"Thứ 6", DayOfWeek.Friday},
        {"Thứ 7", DayOfWeek.Saturday},
    };

    public string CourseId
    {
        get; set;
    }

    public string CourseName
    {
        get; set;
    }

    public string CourseUrl
    {
        get; set;
    }

    public string ClassId
    {
        get; set;
    }

    public int StartPeriod
    {
        get; set;
    }

    public int EndPeriod
    {
        get; set;
    }

    public string Day
    {
        get; set;
    }

    public string Room
    {
        get; set;
    }

    public DateTime From
    {
        get {
            DayOfWeek targetDay = vietnameseDays[Day];
            DateTime today = DateTime.Now;
            int daysToTarget = ((int)targetDay - (int)today.DayOfWeek) % 7;
            DateTime resultDate = today.AddDays(daysToTarget);
            _from = new DateTime(resultDate.Year, resultDate.Month, resultDate.Day, StartPeriod, 0, 0);
            return _from;
        }
        set
        {
            _from = value;
        }
    }

    public DateTime To
    {
        get {
            return _to = new DateTime(_from.Year, _from.Month, _from.Day, EndPeriod + 1, 0, 0);
        }
        set { _to = value; }
    }

    public string ClassDetails => $"{ClassId} - P.{Room}";

    public InformationsForSchedulePage_Lecturer Lecturer
    {
        get; set;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
