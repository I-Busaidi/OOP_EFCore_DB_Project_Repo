using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation
{
    public interface IUsersRepo<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByName(string name);
        T GetByEmail(string email);
        T GetById(int id);
        void Insert(T entity);
        void UpdateById(T entity, int id);
        void DeleteById(int id);
    }
}
