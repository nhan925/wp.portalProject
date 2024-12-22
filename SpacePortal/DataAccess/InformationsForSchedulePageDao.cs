using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using Syncfusion.UI.Xaml.Scheduler;

namespace SpacePortal.DataAccess;
public class InformationsForSchedulePageDao : IDao<InformationsForSchedulePage_Class>
{
    public ObservableCollection<InformationsForSchedulePage_Class> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null)
    {
        keywords ??= new List<string> { "", "" };
        var parameters = new { p_year = keywords[0], p_semester_num = keywords[1] };
        var list_object = App.GetService<ApiService>().Post<List<InformationsForSchedulePage_Class>>("/rpc/get_schedule_page", parameters) ??
            new List<InformationsForSchedulePage_Class>();
        var result = new ObservableCollection<InformationsForSchedulePage_Class>(list_object);
        return result;
    }

    public InformationsForSchedulePage_Class GetById(string id) => throw new NotImplementedException();

    public ObservableCollection<string> GetSemesters()
    {
        var list_semesters = App.GetService<ApiService>().Get<List<string>>("/rpc/get_registered_semesters") ?? new List<string>();
        var result = new ObservableCollection<string>(list_semesters);
        return result;
    }
}
