using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Models;
using SpacePortal.Core.Services;
using SpacePortal.Models;

namespace SpacePortal.DataAccess;
public class InformationsForGradesPageDao : IDao<InformationsForGradesPage_GradesRow>
{
    public ObservableCollection<InformationsForGradesPage_GradesRow> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
         keywords ??= new List<string> { "", "" };
        // keywords[0] is year, keywords[1] is semester
        var parameters = new { p_year = keywords[0], p_semester_num = keywords[1] };
        var list_object = App.GetService<ApiService>().Post<List<InformationsForGradesPage_GradesRow>>("/rpc/get_course_info_by_semester", parameters) ??
            new List<InformationsForGradesPage_GradesRow>();
        var result = new ObservableCollection<InformationsForGradesPage_GradesRow>(list_object);
        return result;
    }
    public InformationsForGradesPage_GradesRow GetById(string id) => throw new NotImplementedException();

    public InformationsForEstimateAverageGradeDialog GetInformationsForEstimateAverageGradeDialog()
    {
        var result = App.GetService<ApiService>().Get<InformationsForEstimateAverageGradeDialog>("/rpc/get_degree_type") ??
            new InformationsForEstimateAverageGradeDialog();
        return result;
    }
}
