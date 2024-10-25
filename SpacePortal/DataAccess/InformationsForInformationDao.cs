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
    public void Add(InformationsForInformation entity)
    {

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public ObservableCollection<InformationsForInformation> GetAll()
    {
        throw new NotImplementedException();
    }

    public InformationsForInformation GetById(int id)
    {
        return App.GetService<ApiService>().Get<InformationsForInformation>("/rpc/get_informations_info");
    }


    public void Update(InformationsForInformation entity)
    {
        throw new NotImplementedException();
    }
}
