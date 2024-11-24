using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Core.Contracts;
public interface IDao<T>
{
    T GetById(string id); // Get an entity by its ID
    ObservableCollection<T> GetAll(int? pageNumber = null, int? pageSize = null, List<string>? keywords = null); // Get all entities
}
