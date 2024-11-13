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
        T GetByName(string fname, string lname);
        void Insert(T entity);
        void UpdateByName(T entity, string fname, string lname);
        void DeleteById(int id);
    }
}
