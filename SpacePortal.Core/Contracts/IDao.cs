using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Core.Contracts;
public interface IDao<T>
{
    T GetById(int id); // Get an entity by its ID
    ObservableCollection<T> GetAll(); // Get all entities
    void Add(T entity); // Add a new entity
    void Update(T entity); // Update an existing entity
    void Delete(int id); // Delete an entity by its ID
}
