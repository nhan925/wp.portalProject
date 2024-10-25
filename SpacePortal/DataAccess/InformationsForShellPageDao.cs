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
    public void Add(InformationsForShellPage entity) => throw new NotImplementedException();
    public void Delete(int id) => throw new NotImplementedException();
    public ObservableCollection<InformationsForShellPage> GetAll() => throw new NotImplementedException();
    public InformationsForShellPage GetById(int id)
    {
        var data = App.GetService<ApiService>().Get<InformationsForShellPage>("/rpc/get_shellpage_info");
        return data;
    }
    public void Update(InformationsForShellPage entity) => throw new NotImplementedException();
}
