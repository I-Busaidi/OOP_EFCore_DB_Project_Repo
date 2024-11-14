using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation
{
    public class UserAccess
    {
        private readonly UserRepo userRepo;
        private readonly BookRepo bookRepo;
        private readonly BorrowRepo borrowRepo;
        private readonly CategoryRepo categoryRepo;

        public UserAccess(UserRepo userRepository, BookRepo bookRepository, BorrowRepo borrowRepository, CategoryRepo categoryRepository)
        {
            userRepo = userRepository;
            bookRepo = bookRepository;
            borrowRepo = borrowRepository;
            categoryRepo = categoryRepository;
        }

        public void RegisterUser(User user)
        {
            userRepo.Insert(user);
        }

        public User LoginUser(string passcode)
        {
            return userRepo.GetAll().FirstOrDefault(u => u.Passcode == passcode);
        }

        public IEnumerable<Book> ViewAllBooks()
        {
            return bookRepo.GetAll();
        }

        public IEnumerable<Category> ViewAllCategories()
        {
            return categoryRepo.GetAll();
        }

        public void BorrowBook(int userId, int bookId)
        {
            var book = bookRepo.GetAll().FirstOrDefault(b => b.BookId == bookId);
            if (book != null && book.TotalCopies - book.BorrowedCopies > 0)
            {
                borrowRepo.Insert(new Borrow
                {
                    BookId = bookId,
                    UserId = userId,
                    BorrowDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(14),
                    IsReturned = false
                });
                book.BorrowedCopies++;
                bookRepo.UpdateByName(book, book.BookName);
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            var borrow = borrowRepo.GetAll().FirstOrDefault(b => b.UserId == userId && b.BookId == bookId && !b.IsReturned);
            if (borrow != null)
            {
                borrow.IsReturned = true;
                borrow.ActualReturnDate = DateTime.Now;
                borrowRepo.UpdateById(borrow, borrow.BorrowId);

                var book = bookRepo.GetAll().FirstOrDefault(b => b.BookId == bookId);
                if (book != null)
                {
                    book.BorrowedCopies--;
                    bookRepo.UpdateByName(book, book.BookName);
                }
            }
        }
    }
}
