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

        public bool IsMasterAdmin(Admin admin)
        {
            return admin.MasterAdminId == null;
        }

        public void RegisterAdmin(Admin admin)
        {
            if (adminRepo.GetByEmail(admin.AdminEmail) == null)
            {
                adminRepo.Insert(admin);
            }
            else
            {
                Console.WriteLine("Admin email already exists.");
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
            adminRepo.DeleteById(id);
        }

        public void UpdateAdmin(Admin admin)
        {
            if (adminRepo.GetByEmail(admin.AdminEmail) == null || adminRepo.GetById(admin.AdminId).AdminEmail == admin.AdminEmail)
            {
                adminRepo.UpdateById(admin, admin.AdminId);
            }
            else
            {
                Console.WriteLine("admin with this email already exists.");
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
            }
            else
            {
                Console.WriteLine("A book with this name already exists.");
            }
        }

        public void UpdateBook(string name, Book book)
        {
            bookRepo.UpdateByName(book, name);
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
            }
            else
            {
                Console.WriteLine("Book not found or currently borrowed.");
            }
        }

        public void AddCategory(Category category)
        {
            categoryRepo.Insert(category);
        }

        public void UpdateCategory(Category category, string catName)
        {
            if (category != null)
            {
                categoryRepo.UpdateByName(category, catName);
            }
        }

        public void DeleteCategory(string catName)
        {
            var category = categoryRepo.GetByName(catName);
            if (category != null && !category.Books.Any())
            {
                categoryRepo.DeleteById(category.CatId);
            }
            else
            {
                Console.WriteLine("Category has books or not found.");
            }
        }

        public void AddUser(User user)
        {
            if (userRepo.GetByEmail(user.Email) == null)
            {
                userRepo.Insert(user);
            }
            else
            {
                Console.WriteLine("Email is already in use.");
            }
        }

        public void RemoveUser(int uId)
        {
            var user = userRepo.GetById(uId);
            if (user != null && !user.Borrows.Any(b => !b.IsReturned))
            {
                userRepo.DeleteById(uId);
            }
            else
            {
                Console.WriteLine("User not found, or has pending returns.");
            }
        }

        public void EditUser(User user)
        {
            if (userRepo.GetByEmail(user.Email) == null || userRepo.GetById(user.UserId).Email == user.Email)
            {
                userRepo.UpdateById(user, user.UserId);
            }
            else
            {
                Console.WriteLine("This user email is already in use.");
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
    }
}
