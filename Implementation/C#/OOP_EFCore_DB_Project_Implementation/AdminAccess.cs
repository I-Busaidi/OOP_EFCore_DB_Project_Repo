using OOP_EFCore_DB_Project_Implementation.Models;
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

        public bool IsMasterAdmin(int id)
        {
            var admin = adminRepo.GetAll().FirstOrDefault(a => a.MasterAdminId == id);

            if (admin == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void RegisterAdmin(Admin admin)
        {
            adminRepo.Insert(admin);
        }

        public Admin LoginAdmin(string email, string password)
        {
            return adminRepo.GetAll().FirstOrDefault(a => a.AdminEmail == email && a.AdminPasscode == password);
        }

        public void AddBook(Book book)
        {
            bookRepo.Insert(book);
        }

        public void UpdateBook(string name, Book book)
        {
            if (!borrowRepo.GetAll().Any(b => b.BookId == book.BookId && b.IsReturned == false))
            {
                bookRepo.UpdateByName(book, name);
            }
        }

        public void DeleteBook(int id)
        {
            if (!borrowRepo.GetAll().Any(b => b.BookId == id && b.IsReturned == false))
            {
                bookRepo.DeleteById(id);
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
    }
}
