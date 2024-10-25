using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using Syncfusion.XlsIO.Implementation;

namespace SpacePortal.DataAccess;

public class InformationsForDashboardDao : IDao<InformationsForDashboard>
{
    public void Add(InformationsForDashboard entity) => throw new NotImplementedException();

    public void Delete(int id) => throw new NotImplementedException();

    public ObservableCollection<InformationsForDashboard> GetAll() => throw new NotImplementedException();

    public InformationsForDashboard GetById(int id)
    {
        // call api
        return App.GetService<ApiService>().Get<InformationsForDashboard>("/rpc/get_dashboard_info");
    }

    public void Update(InformationsForDashboard entity) => throw new NotImplementedException();
}
