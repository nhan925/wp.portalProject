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
public class PaymentHistoryInformationsDao : IDao<PaymentHistoryInfomations>
{
    public ObservableCollection<PaymentHistoryInfomations> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
        var result = App.GetService<ApiService>().Get<List<PaymentHistoryInfomations>>("/rpc/get_payment_history");
        return new ObservableCollection<PaymentHistoryInfomations>(result);
    }
    public PaymentHistoryInfomations GetById(string id) => throw new NotImplementedException();
}
