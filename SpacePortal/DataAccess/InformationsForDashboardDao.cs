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
            StudentFullName = "Nguyễn Trọng Nhân",
            GPAs = new ObservableCollection<KeyValuePair<string, double>>
            {
                new("HK1/22-23", 9.4),
                new("HK2/22-23", 9.8),
                new("HK1/23-24", 10),
                new("HK2/23-24", 9.6),
                new("HK1/24-25", 9.2),
                new("HK1/22-23", 9.4),
                new("HK2/22-23", 9.8),
                new("HK1/23-24", 10),
                new("HK2/23-24", 9.6),
                new("HK1/24-25", 9.2),
                new("HK1/22-23", 9.4),
                new("HK2/22-23", 9.8),
                new("HK1/23-24", 10),
                new("HK2/23-24", 9.6),
                new("HK1/24-25", 9.2),
            },
            CGPA = 9.3,
            CurrentCredit = 24,
            TotalCredit = 120,
        };


    }
    public void Update(InformationsForDashboard entity) => throw new NotImplementedException();
}
