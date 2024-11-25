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
public class InformationsForRequestDetailPageDao : IDao<InformationsForRequestDetailPage_Answer>
{
    public ObservableCollection<InformationsForRequestDetailPage_Answer> GetAll(int? pageNumber = null, int? pageSize = null, List<string> keywords = null) => throw new NotImplementedException();

    public InformationsForRequestDetailPage_Answer GetById(string id)
    {
        var parameters = new { req_id = Convert.ToInt32(id) };
        var result = App.GetService<ApiService>().Post<InformationsForRequestDetailPage_Answer>($"/rpc/get_answer_request", parameters);
        return result;
    }
}