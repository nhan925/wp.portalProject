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
public class ChooseCoursesInformationsDao : IDao<ChooseCoursesInformations>
{
    public ObservableCollection<ChooseCoursesInformations> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null) => throw new NotImplementedException();
    public ChooseCoursesInformations GetById(string id)
    {
        var idInt = int.Parse(id);
        var result = App.GetService<ApiService>().Post<ChooseCoursesInformations>("/rpc/get_choose_courses_information", new { period_id = idInt }) 
            ?? new ChooseCoursesInformations();
        return result;
    }
}
