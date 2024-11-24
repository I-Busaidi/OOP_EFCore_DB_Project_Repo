using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<User> GetByName(string name)
        {
            return _context.Users.Where( u => u.FName.Contains(name) || u.LName.Contains(name) || name.Contains(u.FName) || name.Contains(u.LName));
        }

        public User GetByEmail(string email)
        {
            return _context.Users.Include(b => b.Borrows).FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.Include(b => b.Borrows).Where(u => u.UserId == id).FirstOrDefault();
        }

        public void Insert(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public void UpdateById(User user, int id)
        {
            var userToUpdate = GetById(id);
            if (userToUpdate != null)
            {
                try
                {
                    userToUpdate.FName = user.FName;
                    userToUpdate.LName = user.LName;
                    userToUpdate.Email = user.Email;
                    userToUpdate.Gender = user.Gender;
                    userToUpdate.Passcode = user.Passcode;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }

        public void DeleteById(int uId)
        {
            var user = _context.Users.Find(uId);
            if (user != null)
            {
                try
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }

        public int CountByGender(string gender)
        {
            return _context.Users.Count(u => u.Gender == gender);
        }
    }
}
