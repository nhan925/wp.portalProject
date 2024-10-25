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
        var result = new InformationsForEstimateAverageGradeDialog
        {
            TotalCredits = 138,

            // No need to take the lowest, because you do not want to calculate
            // the average grade for earning the worst degree !!!
            DegreeTypesWithTheirGrade = new Dictionary<string, double>
            {
                { "Xuất sắc", 9.0 },
                { "Giỏi", 8.0 },
                { "Khá", 7.0 },
                { "Trung bình", 5.0 },
            }
        };

        return result;
    }
}
