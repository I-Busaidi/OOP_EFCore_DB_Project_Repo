using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class UserRepo : IUsersRepo<User>
    {
        private readonly LibraryAppDbContext _context;
        public UserRepo(LibraryAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByName(string fname, string lname)
        {
            return _context.Users.FirstOrDefault(u => u.FName == fname && u.LName == lname);
        }

        public void Insert(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateByName(User user, string fname, string lname)
        {
            var userToUpdate = GetByName(fname, lname);
            if (userToUpdate != null)
            {
                _context.Entry(userToUpdate).CurrentValues.SetValues(user);
                _context.SaveChanges();
            }
        }

        public void DeleteById(int uId)
        {
            var user = _context.Users.Find(uId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public int CountByGender(string gender)
        {
            return _context.Users.Count(u => u.Gender == gender);
        }
    }
}
