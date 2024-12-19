using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class InformationsForAIChatbot : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Message
    {
        get; set;
    }

    public bool IsUser
    {
        get; set;
    }
}
