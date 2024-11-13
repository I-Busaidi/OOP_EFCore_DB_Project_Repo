using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class BookRepo : IObjectsRepo<Book>
    {
        private readonly LibraryAppDbContext _context;
        public BookRepo(LibraryAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }
        public Book GetByName(string name)
        {
            return _context.Books.FirstOrDefault(book => book.BookName == name);
        }
        public void Insert(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        public void UpdateByName(Book book, string name)
        {
            var bookToUpdate = GetByName(name);
            if (bookToUpdate != null)
            {
                _context.Books.Entry(bookToUpdate).CurrentValues.SetValues(book);
                _context.SaveChanges();
            }
        }
        public void DeleteById(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
        public decimal GetTotalPrice()
        {
            return _context.Books.Sum(book => book.CopyPrice);
        }
        public decimal GetMaxPrice()
        {
            return _context.Books.Max(book => book.CopyPrice);
        }
        public int GetTotalBorrowedBooks()
        {
            return _context.Borrows.Count(b => b.IsReturned == false);
        }
        public int GetTotalBooksPerCategoryName(string name)
        {
            return _context.Books.Count(book => book.Category.CatName == name);
        }
    }
}
