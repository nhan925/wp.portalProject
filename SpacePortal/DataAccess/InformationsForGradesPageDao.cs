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

    public void Add(InformationsForGradesPage_GradesRow entity) => throw new NotImplementedException();

    public void Delete(int id) => throw new NotImplementedException();

    public void Update(InformationsForGradesPage_GradesRow entity) => throw new NotImplementedException();

    public InformationsForGradesPage_GradesRow GetById(int id) => throw new NotImplementedException();

    public ObservableCollection<InformationsForGradesPage_GradesRow> GetAll()
    {
        
        var parameters = new { p_year = "", p_semester_num = "" };
        List<InformationsForGradesPage_GradesRow> list_object = App.GetService<ApiService>().Post<List<InformationsForGradesPage_GradesRow>>("/rpc/get_course_info_by_semester",parameters);
        ObservableCollection<InformationsForGradesPage_GradesRow> result = new ObservableCollection<InformationsForGradesPage_GradesRow>(list_object);
        return result;
    }

    public InformationsForEstimateAverageGradeDialog GetInformationsForEstimateAverageGradeDialog()
    {
        return App.GetService<ApiService>().Get<InformationsForEstimateAverageGradeDialog>("/rpc/get_degree_type");
    }
}
