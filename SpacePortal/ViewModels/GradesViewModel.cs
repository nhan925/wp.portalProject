using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.DataAccess;
using SpacePortal.Core.Models;

namespace SpacePortal.ViewModels;

public partial class GradesViewModel : ObservableRecipient
{

     public ObservableCollection<GradesRow> Grades
     {
         get; set;
     } = new ObservableCollection<GradesRow>();

    public void Init()
     {
         IDao<GradesRow> dao = new GradesPageMockDao();
         Grades = dao.GetAll();
     }

    public GradesViewModel()
    {
   
    }
}
