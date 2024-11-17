using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;

namespace OOP_EFCore_DB_Project_Implementation
{
    internal class Program
    {
        private static int? currentUserId = null;
        private static bool isUserLoggedIn = false;
        private static bool isAdminLoggedIn = false;
        private static bool isMasterAdmin = false;
        private static UserAccess userAccess;
        private static AdminAccess adminAccess;
        private static LibraryAppDbContext dbContext;

        static void Main(string[] args)
        {
            dbContext = new LibraryAppDbContext();
            var adminRepository = new AdminRepo(dbContext);
            var userRepository = new UserRepo(dbContext);
            var bookRepository = new BookRepo(dbContext);
            var categoryRepository = new CategoryRepo(dbContext);
            var borrowRepository = new BorrowRepo(dbContext);
            adminAccess = new AdminAccess(adminRepository, userRepository, bookRepository, categoryRepository, borrowRepository);
            userAccess = new UserAccess(userRepository, bookRepository, borrowRepository, categoryRepository);

            Console.WriteLine("Welcome to the Library Management System!");
            ShowLoginMenu();
        }

        private static void ShowLoginMenu()
        {
            string header = "Select an option to login:";
            string[] loginOptions = { "Admin Login", "User Login", "Exit" };
            int choice = ArrowKeySelection(loginOptions.ToList(), header);

            if (choice == 0)
            {
                AdminLogin();
            }
            else if (choice == 1)
            {
                UserLogin();
            }
            else if (choice == 2)
            {
                return;
            }
        }

        private static void AdminLogin()
        {
            Console.Clear();
            Console.WriteLine("Enter Admin Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter Admin Password:");
            string password = Console.ReadLine();

            var admin = adminAccess.LoginAdmin(email, password);
            if (admin != null)
            {
                isAdminLoggedIn = true;
                isMasterAdmin = admin.MasterAdminId == null;
                currentUserId = admin.AdminId;
                Console.WriteLine("Login successful!");
                ShowAdminMenu(admin);
            }
            else
            {
                Console.WriteLine("Invalid credentials. Try again.");
                Console.ReadLine();
            }
        }

        private static void UserLogin()
        {
            Console.Clear();
            Console.WriteLine("Enter User Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter User Password:");
            string password = Console.ReadLine();

            var user = userAccess.LoginUser(email, password);
            if (user != null)
            {
                currentUserId = user.UserId;
                isUserLoggedIn = true;
                Console.WriteLine("Login successful!");
                ShowUserMenu(user);
            }
            else
            {
                Console.WriteLine("Invalid credentials. Try again.");
                Console.ReadLine();
            }
        }

        private static void ShowAdminMenu(Admin admin)
        {
            string header = "Admin Menu:";
            string[] options = { "Add Book", "View All Books", "View Users", "Manage Categories", "Search for Books", "Logout" };

            int choice = ArrowKeySelection(options.ToList(), header);
            if (choice == 0)
            {
                AddBookMenu();
            }
            else if (choice == 1)
            {
                ViewAllBooks();
            }
            else if (choice == 2)
            {
                ViewAllUsers();
            }
            else if (choice == 3)
            {
                ManageCategories();
            }
            else if (choice == 4)
            {
                SearchBooksMenu();
            }
            else if (choice == 5)
            {
                Logout();
                return;
            }

        }

        private static void ShowUserMenu(User user)
        {
            string header = "User Menu:";
            string[] options = {"Browse Books", "Search for Books", "View Borrowed Books", "Edit User Info", "Logout"};

            int choice = ArrowKeySelection(options.ToList(), header);
            if (choice == 0)
            {
                BrowseBooks();
            }
            else if (choice == 1)
            {
                SearchBooksMenu();
            }
            else if (choice == 2)
            {
                ViewBorrowedBooks(user);
            }
            else if (choice == 3)
            {
                EditUserInfo(user);
            }
            else if (choice == 4)
            {
                Logout();
                return;
            }

        }

        private static void BrowseBooks()
        {
            var books = userAccess.ViewAllBooks();
            if (books != null && books.Any())
            {
                var bookList = books.ToList();
                Console.Clear();
                string header = "Select a Book to Borrow:";

                int selectedIndex = ArrowKeySelection(bookList.Select(b => $"{b.BookName} by {b.AuthorName} | Available copies: {b.TotalCopies - b.BorrowedCopies}").ToList(), header);

                var selectedBook = bookList[selectedIndex];
                userAccess.BorrowBook(currentUserId.Value, selectedBook.BookId);
                ShowRecommendations(selectedBook);
            }
            else
            {
                Console.WriteLine("No books available for browsing.");
            }
        }


        private static void ShowRecommendations(Book book)
        {
            var recommendedBooks = userAccess.RecommendedBooks(currentUserId.Value);
            Console.Clear();
            string header = "Recommended Books:";

            var random = new Random();
            var randomBooks = recommendedBooks.OrderBy(x => random.Next()).Take(5).ToList();
            int selectedBookIndex = ArrowKeySelection(randomBooks.Select(b => $"{b.BookName} by {b.AuthorName}").ToList(), header);

            if (selectedBookIndex >= 0 && selectedBookIndex < randomBooks.Count)
            {
                var selectedBook = randomBooks[selectedBookIndex];
                userAccess.BorrowBook(currentUserId.Value, selectedBook.BookId);
            }
        }

        private static int ArrowKeySelection(List<string> options, string head)
        {
            int selectedIndex = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(head+"\n\n");
                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.WriteLine($">> {options[i]} <<");
                    }
                    else
                    {
                        Console.WriteLine($"   {options[i]}");
                    }
                }
                Console.WriteLine("\n\nUse arrow keys to select.");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex > 0)? selectedIndex -1 : options.Count -1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex < options.Count -1)? selectedIndex + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return selectedIndex;
                }
            }
        }

        private static void AddBookMenu()
        {
            Console.Clear();
            Console.WriteLine("Enter Book Name:");
            string bookName = Console.ReadLine();
            Console.WriteLine("Enter Author Name:");
            string authorName = Console.ReadLine();

            var categories = adminAccess.ViewAllCategories();
            string header = "Select a category for the book:";
            var selectedCategoryIndex = ArrowKeySelection(categories.Select(c => c.CatName).ToList(), header);
            var selectedCategory = categories.ElementAt(selectedCategoryIndex);

            var book = new Book
            {
                BookName = bookName,
                AuthorName = authorName,
                CatId = selectedCategory.CatId
            };

            adminAccess.AddBook(book);
        }

        private static void SearchBooksMenu()
        {
            Console.Clear();
            Console.WriteLine("Enter Book Name or Author Name to Search:");
            string searchTerm = Console.ReadLine();

            var books = userAccess.SearchBooks(searchTerm);
            foreach (var book in books)
            {
                Console.WriteLine($"{book.BookName} by {book.AuthorName}");
            }
            Console.ReadLine();
        }

        private static void ViewAllBooks()
        {
            Console.Clear();
            var books = adminAccess.ViewAllBooks();
            foreach (var book in books)
            {
                Console.WriteLine($"{book.BookName} by {book.AuthorName}");
            }
            Console.ReadLine();
        }

        private static void ViewAllUsers()
        {
            Console.Clear();
            var users = adminAccess.ViewAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Email}");
            }
            Console.ReadLine();
        }

        private static void ManageCategories()
        {
        }

        private static void ViewBorrowedBooks(User user)
        {
            Console.Clear();
            var borrowedBooks = userAccess.GetCurrentBorrows(user.UserId);
            foreach (var book in borrowedBooks)
            {
                Console.WriteLine($"{book.Book.BookName} by {book.Book.AuthorName}");
            }
            Console.ReadLine();
        }

        private static void EditUserInfo(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter new email:");
            string newEmail = Console.ReadLine();
            userAccess.EditInfo(user);
        }

        private static void Logout()
        {
            isUserLoggedIn = false;
            isAdminLoggedIn = false;
            currentUserId = null;
            ShowLoginMenu();
        }
    }
}