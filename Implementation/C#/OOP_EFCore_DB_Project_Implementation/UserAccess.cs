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
            if (userRepo.GetByEmail(user.Email) == null)
            {
                userRepo.Insert(user);
            }
            else
            {
                Console.WriteLine("This user email is already in use.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void EditInfo(User user)
        {
            if (userRepo.GetByEmail(user.Email) == null || userRepo.GetById(user.UserId).Email == user.Email)
            {
                userRepo.UpdateById(user, user.UserId);
            }
            else
            {
                Console.WriteLine("This user email is already in use.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public User LoginUser(string userEmail, string passcode)
        {
            return userRepo.GetAll().FirstOrDefault(u => u.Passcode == passcode && u.Email == userEmail);
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
            var user = userRepo.GetById(userId);
            var book = bookRepo.GetAll().FirstOrDefault(b => b.BookId == bookId);
            if (book != null && book.TotalCopies > book.BorrowedCopies)
            {
                if (!user.Borrows.Any(b => b.BookId == bookId && !b.IsReturned))
                {
                    if (!user.Borrows.Any(b => !b.IsReturned && b.ReturnDate < DateTime.Now))
                    {
                        borrowRepo.Insert(new Borrow
                        {
                            BookId = bookId,
                            UserId = userId,
                            BorrowDate = DateTime.Now,
                            ReturnDate = DateTime.Now.AddDays(book.BorrowPeriod),
                            IsReturned = false
                        });
                        book.BorrowedCopies++;
                        bookRepo.UpdateByName(book, book.BookName);
                    }
                    else
                    {
                        Console.WriteLine("User has overdue books that must be returned first.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("User is currently borrowing this book already.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("No copies available for borrowing.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void ReturnBook(int userId, int bookId, int rating)
        {
            var borrow = borrowRepo.GetAll().FirstOrDefault(b => b.UserId == userId && b.BookId == bookId && !b.IsReturned);
            if (borrow != null)
            {
                borrow.IsReturned = true;
                borrow.ActualReturnDate = DateTime.Now;
                borrow.Rating = rating;
                borrowRepo.UpdateById(borrow, borrow.BorrowId);

                var book = bookRepo.GetAll().FirstOrDefault(b => b.BookId == bookId);
                if (book != null)
                {
                    book.BorrowedCopies--;
                    bookRepo.UpdateByName(book, book.BookName);
                }
            }
        }

        public IEnumerable<Borrow> GetCurrentBorrows(int userId) 
        { 
            return borrowRepo.GetUserBorrowsById(userId).Where(b => !b.IsReturned); 
        }

        public IEnumerable<Borrow> GetBorrowHistory(int userId) 
        { 
            return borrowRepo.GetUserBorrowsById(userId); 
        }

        public IEnumerable<Book> RecommendedBooks(int uId)
        {
            var userBorrows = borrowRepo.GetUserBorrowsById(uId);
            var borrowCategory = userBorrows.Select(b => b.Book.CatId).Distinct();
            var recommendedBooks = bookRepo.GetAll()
                                           .Where(b => borrowCategory
                                           .Contains(b.CatId) && b.BorrowedCopies < b.TotalCopies)
                                           .ToList();
            var otherUserBorrows = borrowRepo.GetAll()
                                             .Where(b => borrowCategory.Contains(b.Book.CatId) && b.UserId != uId)
                                             .Select(b => b.Book)
                                             .Distinct()
                                             .ToList();
            recommendedBooks.AddRange(otherUserBorrows.Except(recommendedBooks));
            return recommendedBooks;
        }

        public IEnumerable<Book> SearchBooks(string query) 
        { 
            return bookRepo.GetAll()
                           .Where(b => b.BookName
                           .Contains(query, StringComparison.OrdinalIgnoreCase) 
                           || b.AuthorName.Contains(query, StringComparison.OrdinalIgnoreCase))
                           .ToList(); 
        }

        public IEnumerable<Book> FilterBooksByCategory(string categoryName) 
        {
            return bookRepo.GetAll()
                           .Where(b => b.Category.CatName
                           .Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                           .ToList(); 
        }
    }
}
