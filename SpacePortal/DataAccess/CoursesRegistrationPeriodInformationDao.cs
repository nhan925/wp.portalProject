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
public class CoursesRegistrationPeriodInformationDao : IDao<CoursesRegistrationPeriodInformation>
{
    public ObservableCollection<CoursesRegistrationPeriodInformation> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
        var result = App.GetService<ApiService>().Get<List<CoursesRegistrationPeriodInformation>>("/rpc/get_courses_registration_period") 
            ?? new List<CoursesRegistrationPeriodInformation>();
        return new ObservableCollection<CoursesRegistrationPeriodInformation>(result);
    }

    public CoursesRegistrationPeriodInformation GetById(string id) => throw new NotImplementedException();
}
