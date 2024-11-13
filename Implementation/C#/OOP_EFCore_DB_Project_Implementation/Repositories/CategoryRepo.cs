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
            return _context.Categories.ToList();
        }
        public Category GetByName(string name)
        {
            return _context.Categories.FirstOrDefault(c => c.CatName == name);
        }
        public void Insert(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void UpdateByName(Category category, string name)
        {
            var categoryToUpdate = GetByName(name);
            if (categoryToUpdate != null)
            {
                _context.Categories.Entry(categoryToUpdate).CurrentValues.SetValues(category);
                _context.SaveChanges();
            }
        }
        public void DeleteById(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
