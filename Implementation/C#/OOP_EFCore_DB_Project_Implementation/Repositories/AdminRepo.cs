using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class AdminRepo : IUsersRepo<Admin>
    {
        private readonly LibraryAppDbContext _context;
        public AdminRepo(LibraryAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }
        public Admin GetByName(string firstName, string? lastName)
        {
            return _context.Admins.FirstOrDefault(a => a.AdminFname == firstName
                                                    && a.AdminLname == lastName);
        }

        public Admin GetByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(a => a.AdminEmail == email);
        }

        public Admin GetById(int id)
        {
            return _context.Admins.Find(id);
        }

        public void Insert(Admin admin)
        {
            try
            {
                _context.Admins.Add(admin);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        public void UpdateByName(Admin admin, string firstName, string? lastName)
        {
            var adminToUpdate = GetByName(firstName, lastName);
            if (adminToUpdate != null)
            {
                try
                {
                    _context.Admins.Entry(adminToUpdate).CurrentValues.SetValues(admin);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
        public void DeleteById(int id)
        {
            var admin = _context.Admins.Find(id);
            if (admin != null)
            {
                try
                {
                    _context.Admins.Remove(admin);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}
