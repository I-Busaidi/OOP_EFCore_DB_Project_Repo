using Microsoft.EntityFrameworkCore;
using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class BorrowRepo
    {
        private readonly LibraryAppDbContext _context;
        public BorrowRepo(LibraryAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Borrow> GetAll()
        {
            return _context.Borrows.ToList();
        }

        public IEnumerable<Borrow> GetAllDetails()
        {
            return _context.Borrows.Include(b => b.Book)
                                   .Include(u => u.User)
                                   .ToList();
        }

        public Borrow GetById(int id)
        {
            return _context.Borrows.Include(b => b.Book)
                                   .Include(u => u.User)
                                   .FirstOrDefault(br => br.BorrowId == id);
        }

        public IEnumerable<Borrow> GetUserBorrowsById(int userId)
        {
            return _context.Borrows.Include(b => b.Book)
                                   .Include(u => u.User)
                                   .Where(br => br.UserId == userId)
                                   .ToList();
        }

        public IEnumerable<Borrow> GetBookBorrowsById(int bookId)
        {
            return _context.Borrows.Include(b => b.Book)
                                   .Where(br => br.BookId == bookId)
                                   .ToList();
        }

        public int GetCurrentBorrowedBooks()
        {
            return _context.Borrows.Where(b => b.IsReturned == false).Count();
        }

        public void Insert(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            _context.SaveChanges();
        }

        public void UpdateById(Borrow borrow, int id)
        {
            var borrowToUpdate = GetById(id);
            if (borrowToUpdate != null)
            {
                _context.Borrows.Entry(borrowToUpdate).CurrentValues.SetValues(borrow);
                _context.SaveChanges();
            }
        }

        public void DeleteById(int id)
        {
            var borrow = GetById(id);
            if(borrow != null)
            {
                _context.Borrows.Remove(borrow);
                _context.SaveChanges();
            }
        }
    }
}
