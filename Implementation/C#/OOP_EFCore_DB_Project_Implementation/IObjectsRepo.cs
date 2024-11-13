using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation
{
    public interface IObjectsRepo<T>
    {
        IEnumerable<T> GetAll();
        T GetByName(string name);
        void Insert(T entity);
        void UpdateByName(T entity, string name);
        void DeleteById(int id);
    }
}
