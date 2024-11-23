using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.DataAccess;
public class InformationsForRequestPageDao : IDao<InformationsForRequest_RequestRow>
{
    public ObservableCollection<InformationsForRequest_RequestRow> GetAll(int? pageNumber = null, int? pageSize = null, List<string> keywords = null)
    {
        pageNumber = pageNumber ?? 1;
        pageSize = pageSize ?? 10;
        var pskip = (pageNumber - 1) * pageSize;
        var ptake = pageSize;
        var parameters = new { skip = pskip, take = ptake };
        var list_object = App.GetService<ApiService>().Post<List<InformationsForRequest_RequestRow>>("/rpc/get_all_request_data",parameters) ??
            new List<InformationsForRequest_RequestRow>();
        var result = new ObservableCollection<InformationsForRequest_RequestRow>(list_object);
        return result;
    }
    public InformationsForRequest_RequestRow GetById(string id) => throw new NotImplementedException();
}
