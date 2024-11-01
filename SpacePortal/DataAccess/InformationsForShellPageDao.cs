using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.DataAccess;
public class InformationsForShellPageDao : IDao<InformationsForShellPage>
{
    public InformationsForShellPage GetById(string id)
    {
        var data = App.GetService<ApiService>().Get<InformationsForShellPage>("/rpc/get_shellpage_info") ??
            new InformationsForShellPage();
        return data;
    }

    ObservableCollection<InformationsForShellPage> IDao<InformationsForShellPage>.GetAll(int? pageNumber, int? pageSize, List<string> keywords) => throw new NotImplementedException();
}
