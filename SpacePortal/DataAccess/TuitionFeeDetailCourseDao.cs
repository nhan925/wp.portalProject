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
public class TuitionFeeDetailCourseDao : IDao<TuitionFeeDetailCourse>
{
    public ObservableCollection<TuitionFeeDetailCourse> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
        => throw new NotImplementedException();

    public ObservableCollection<TuitionFeeDetailCourse> GetCourseList(string year, int semester)
    {
        var result = App.GetService<ApiService>().Post<List<TuitionFeeDetailCourse>>("/rpc/get_payment_course_detail", new
        {
            v_year = year,
            v_semester_num = semester
        })
        ?? new List<TuitionFeeDetailCourse>();

        return new ObservableCollection<TuitionFeeDetailCourse>(result);
    }

    public TuitionFeeDetailCourse GetById(string id) => throw new NotImplementedException();
}
