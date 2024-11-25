using Microsoft.EntityFrameworkCore;
using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class CategoryRepo : IObjectsRepo<Category>
    {
        private readonly LibraryAppDbContext _context;
        public CategoryRepo (LibraryAppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Books).ToList();
        }
        public Category GetByName(string name)
        {
            return _context.Categories.Include(b => b.Books).Where(c => c.CatName == name).FirstOrDefault();
        }
        public void Insert(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void UpdateByName(Category category, string name)
        {
            var categoryToUpdate = GetByName(name);
            if (categoryToUpdate != null)
            {
                try
                {
                    categoryToUpdate.CatName = category.CatName;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
        public void IncrementBookNo(Category category, string name)
        {
            var categoryToUpdate = GetByName(name);
            if (categoryToUpdate != null)
            {
                try
                {
                    categoryToUpdate.NumOfBooks += 1;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
        public void DecrementBookNo(Category category, string name)
        {
            var categoryToUpdate = GetByName(name);
            if (categoryToUpdate != null)
            {
                try
                {
                    categoryToUpdate.NumOfBooks -= 1;
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
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                try
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
