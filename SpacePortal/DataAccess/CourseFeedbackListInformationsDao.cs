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
public class CourseFeedbackListInformationsDao : IDao<CourseFeedbackListInformations>
{
    public ObservableCollection<CourseFeedbackListInformations> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
        var result = App.GetService<ApiService>().Get<List<CourseFeedbackListInformations>>("/rpc/get_course_for_feedback")
            ?? new List<CourseFeedbackListInformations>();
        return new ObservableCollection<CourseFeedbackListInformations>(result);

    }
    public CourseFeedbackListInformations GetById(string id) => throw new NotImplementedException();
}

