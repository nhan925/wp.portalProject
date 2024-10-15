using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Models;

namespace SpacePortal.DataAccess;

public class InformationsForDashboardDao : IDao<InformationsForDashboard>
{
    public void Add(InformationsForDashboard entity) => throw new NotImplementedException();
    public void Delete(int id) => throw new NotImplementedException();
    public ObservableCollection<InformationsForDashboard> GetAll() => throw new NotImplementedException();
    public InformationsForDashboard GetById(int id)
    {
        return new InformationsForDashboard
        {
            StudentFullName = "John Doe",
            GPAs = new Dictionary<string, double>
            {
                { "Fall 2021", 9.4 },
                { "Spring 2022", 9.8 }
            },
            CGPA = 9.3,
            TotalCredits = 120,
            CurrentCredits = 30
        };
    }
    public void Update(InformationsForDashboard entity) => throw new NotImplementedException();
}
