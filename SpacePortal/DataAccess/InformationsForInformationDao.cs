using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;


namespace SpacePortal.DataAccess;
public class InformationsForInformationDao : IDao<InformationsForInformation>
{
    public InformationsForInformation GetById(string id)
    {
        var result = App.GetService<ApiService>().Get<InformationsForInformation>("/rpc/get_informations_info") ??
            new InformationsForInformation();
        return result;
    }

    ObservableCollection<InformationsForInformation> IDao<InformationsForInformation>.GetAll(int? pageNumber, int? pageSize, List<string> keywords) => throw new NotImplementedException();
}
