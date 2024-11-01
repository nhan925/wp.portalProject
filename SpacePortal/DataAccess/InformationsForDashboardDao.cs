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
   
    public InformationsForDashboard GetById(string id)
    {
        var result = App.GetService<ApiService>().Get<InformationsForDashboard>("/rpc/get_dashboard_info") ?? new InformationsForDashboard();
        return result;
    }

    ObservableCollection<InformationsForDashboard> IDao<InformationsForDashboard>.GetAll(int? pageNumber, int? pageSize, List<string> keywords) => throw new NotImplementedException();
}
