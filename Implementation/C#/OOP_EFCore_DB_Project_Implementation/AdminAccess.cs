﻿using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation
{
    public class AdminAccess
    {
        private readonly AdminRepo adminRepo;
        private readonly BookRepo bookRepo;
        private readonly CategoryRepo categoryRepo;
        private readonly UserRepo userRepo;
        private readonly BorrowRepo borrowRepo;

        public AdminAccess(AdminRepo adminRepository, UserRepo userRepository, BookRepo bookRepository, CategoryRepo categoryRepository, BorrowRepo borrowRepository)
        {
            adminRepo = adminRepository;
            bookRepo = bookRepository;
            categoryRepo = categoryRepository;
            userRepo = userRepository;
            borrowRepo = borrowRepository;
        }

        public bool IsMasterAdmin(Admin admin)
        {
            return admin.MasterAdminId == null;
        }

        public void RegisterAdmin(Admin admin)
        {
            if (adminRepo.GetByEmail(admin.AdminEmail) == null)
            {
                adminRepo.Insert(admin);
                Console.WriteLine("Added successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Admin email already exists.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public IEnumerable<Admin> GetAllAdmins()
        {
            return adminRepo.GetAll().ToList();
        }

        public Admin GetMasterAdminSecets() //FOR TESTING ONLY
        {
            return adminRepo.GetAll().FirstOrDefault(a => a.MasterAdminId == null);
        }

        public void RemoveAdmin(int id)
        {
            var mAdmins = adminRepo.GetAll().Where(a => a.MasterAdminId == null).ToList();
            if (mAdmins.Count == 1)
            {
                Console.WriteLine("Cannot remove the last remaining master admin.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                adminRepo.DeleteById(id);
                Console.WriteLine("Removed successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void UpdateAdmin(Admin admin)
        {
            if (adminRepo.GetByEmail(admin.AdminEmail) == null || adminRepo.GetById(admin.AdminId).AdminEmail == admin.AdminEmail)
            {
                adminRepo.UpdateById(admin, admin.AdminId);
                Console.WriteLine("Updated successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("admin with this email already exists.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public Admin LoginAdmin(string email, string password)
        {
            return adminRepo.GetAll().FirstOrDefault(a => a.AdminEmail == email && a.AdminPasscode == password);
        }

        public void AddBook(Book book)
        {
            if (bookRepo.GetByName(book.BookName) == null)
            {
                bookRepo.Insert(book);
                var category = categoryRepo.GetByName(book.Category.CatName);
                if (category != null)
                {
                    category.NumOfBooks++;
                    categoryRepo.UpdateByName(category, category.CatName);
                }
                Console.WriteLine("Added successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("A book with this name already exists.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void UpdateBook(string name, Book book)
        {
            if (bookRepo.GetByName(book.BookName) == null || bookRepo.GetByName(book.BookName).BookId == book.BookId)
            {
                bookRepo.UpdateByName(book, name);
                Console.WriteLine("Updated successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("A book with this name already exists.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void DeleteBook(string name)
        {
            var book = bookRepo.GetByName(name);
            if (book != null && !bookRepo.IsBookBorrowed(book.BookId))
            {
                var category = categoryRepo.GetByName(book.Category.CatName);
                bookRepo.DeleteById(book.BookId);
                if (category != null)
                {
                    category.NumOfBooks--;
                    categoryRepo.UpdateByName(category, category.CatName);
                }
                Console.WriteLine("Removed successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Book not found or currently borrowed.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void AddCategory(Category category)
        {
            categoryRepo.Insert(category);
            Console.WriteLine("Added successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public void UpdateCategory(Category category, string catName)
        {
            var categoryCheck = categoryRepo.GetByName(catName);
            if (categoryCheck == null || categoryCheck.CatId == category.CatId)
            {
                categoryRepo.UpdateByName(category, catName);
                Console.WriteLine("Updated successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("A category with this name already exists.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void DeleteCategory(string catName)
        {
            var category = categoryRepo.GetByName(catName);
            if (category != null && !category.Books.Any())
            {
                categoryRepo.DeleteById(category.CatId);
                Console.WriteLine("Removed successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Category has books or not found.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void AddUser(User user)
        {
            if (userRepo.GetByEmail(user.Email) == null)
            {
                userRepo.Insert(user);
                Console.WriteLine("Added successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Email is already in use.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void RemoveUser(int uId)
        {
            var user = userRepo.GetById(uId);
            if (user != null && !user.Borrows.Any(b => !b.IsReturned))
            {
                userRepo.DeleteById(uId);
                Console.WriteLine("Removed successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("User not found, or has pending returns.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void EditUser(User user)
        {
            if (userRepo.GetByEmail(user.Email) == null || userRepo.GetById(user.UserId).Email == user.Email)
            {
                userRepo.UpdateById(user, user.UserId);
                Console.WriteLine("Updated successfully.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("This user email is already in use.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public IEnumerable<Book> ViewAllBooks()
        {
            return bookRepo.GetAll();
        }

        public IEnumerable<Category> ViewAllCategories()
        {
            return categoryRepo.GetAll();
        }

        public IEnumerable<User> ViewAllUsers()
        {
            return userRepo.GetAll();
        }

        //FOR INITIAL SETUP ONLY
        public void AddBorrow(Borrow borrow)
        {
            borrowRepo.Insert(borrow);
        }

        public IEnumerable<User> GetUsersByName(string name)
        {
            return userRepo.GetByName(name);
        }

        public IEnumerable<Admin> GetAdminsByName(string name)
        {
            return adminRepo.GetByName(name);
        }

        public IEnumerable<Borrow> GetAllBorrows(int pageNumber, int pageSize)
        {
            return borrowRepo.GetAllDetails(pageNumber, pageSize);
        }

        public IEnumerable<(Book book, double avgRating)> GetBooksRating()
        {
            var books = bookRepo.GetAll();
            var ratedBooks = books.Select(book =>
            {
                var bookBorrows = borrowRepo.GetBookBorrowsById(book.BookId);
                var averageRating = bookBorrows.Any() ? bookBorrows.Average(b => b.Rating ?? 0) : 0;
                return (book, averageRating);
            });
            return ratedBooks;
        }
        public IEnumerable<(string CategoryName, int BookCount, decimal TotalCost)> GetBooksCountAndCostPerCategory()
        {
            var categories = categoryRepo.GetAll();
            if (categories == null)
            {
                return Enumerable.Empty<(string, int, decimal)>();
            }

            var result = categories.Select(category =>
            {
                var bookCount = category.Books?.Count ?? 0;
                var totalCost = category.Books?.Sum(book => book.CopyPrice * book.TotalCopies) ?? 0m;
                return (CategoryName: category.CatName, BookCount: bookCount, TotalCost: totalCost);
            });

            return result;
        }

        public decimal GetTotalLibraryCost()
        {
            return bookRepo.GetTotalPrice();
        }

        public int GetUserCountByGender(string gender)
        {
            return userRepo.CountByGender(gender);
        }

        public int GetTotalBorrowedBooks()
        {
            return borrowRepo.GetCurrentBorrowedBooks();
        }

        public decimal GetMaxBookPrice()
        {
            return bookRepo.GetMaxPrice();
        }
    }
}
