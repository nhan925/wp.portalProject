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
public class TuitionFeeListInformationsDao : IDao<TuitionFeeListInformations>
{
    public ObservableCollection<TuitionFeeListInformations> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
        var result = App.GetService<ApiService>().Get<List<TuitionFeeListInformations>>("/rpc/get_fee_for_payment")
            ?? new List<TuitionFeeListInformations>();
        return new ObservableCollection<TuitionFeeListInformations>(result);

    }
    public TuitionFeeListInformations GetById(string id) => throw new NotImplementedException();
}
