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
        public IEnumerable<Admin> GetByName(string name)
        {
            return _context.Admins.Where(a => a.AdminFname.Contains(name) || a.AdminLname.Contains(name));
        }

        public Admin GetByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(a => a.AdminEmail.ToLower() == email.ToLower());
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
        public void UpdateById(Admin admin, int id)
        {
            var adminToUpdate = GetById(id);
            if (adminToUpdate != null)
            {
                try
                {
                    adminToUpdate.AdminEmail = admin.AdminEmail;
                    adminToUpdate.AdminLname = admin.AdminLname;
                    adminToUpdate.AdminFname = admin.AdminFname;
                    adminToUpdate.AdminPasscode = admin.AdminPasscode;
                    adminToUpdate.MasterAdminId = admin.MasterAdminId;
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
