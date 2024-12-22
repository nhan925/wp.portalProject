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
public class InformationsForScholarshipPageDao : IDao<InformationsForScholarshipPage>
{
    public ObservableCollection<InformationsForScholarshipPage> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null) => throw new NotImplementedException();
    public InformationsForScholarshipPage GetById(string id) => throw new NotImplementedException();

    public ObservableCollection<InformationsForScholarshipPage> GetByYear(string year)
    {
        var result = App.GetService<ApiService>().Post<List<InformationsForScholarshipPage>>("/rpc/get_scholarships_with_status", new
        {
            year = year
        })
        ?? new List<InformationsForScholarshipPage>();
        return new ObservableCollection<InformationsForScholarshipPage>(result);
    }
}
