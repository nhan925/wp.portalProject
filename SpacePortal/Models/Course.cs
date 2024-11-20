using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Models;

public class Course
{
    public string Id
    {
        get; set;
    }

    public string Name
    {
        get; set;
    }

    public int Credits
    {
        get; set;
    }

    public string ClassName
    {
        get; set;
    }
}
