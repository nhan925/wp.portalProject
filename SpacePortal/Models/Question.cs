using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;
public class Question
{
    public int Number
    {
        get; set;
    }
    public string Content
    {
        get; set;
    }
    public int SelectedRating
    {
        get; set;
    }
}