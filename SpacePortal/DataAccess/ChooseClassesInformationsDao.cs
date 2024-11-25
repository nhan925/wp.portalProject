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

public class ChooseClassesInformationsDao : IDao<ChooseClassesInformations>
{
    public ObservableCollection<ChooseClassesInformations> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null) => throw new NotImplementedException();
    public ChooseClassesInformations GetById(string id) => throw new NotImplementedException();

    public ChooseClassesInformations GetById(string courseId, string periodId)
    {
        var result = App.GetService<ApiService>().Post<ChooseClassesInformations>("/rpc/get_choose_classes_information", new
        {
            course_id_register = courseId,
            period_id = periodId
        })
            ?? new ChooseClassesInformations();
        return result;
    }
}
