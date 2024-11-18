﻿using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
        public static string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public static string passcodePattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$";

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

            //initial setup:
            if(!adminRepository.GetAll().Any())
            {
                InitialSetup();
            }

            Console.WriteLine("Welcome to the Library Management System!");
            ShowLoginMenu();
        }

        private static void ShowLoginMenu()
        {
            string header = "Select an option to login:";
            string[] loginOptions = { "Admin Login", "User Login", "Register User", "Exit" };
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
                AddUser();
            }
            else if (choice == 3)
            {
                return;
            }
        }

        private static void AddUser()
        {
            Console.Clear();
            Console.Write("Enter the first name: ");
            string fname = Console.ReadLine();
            Console.Write("\nEnter the last name: ");
            string lname = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Enter the user email (example@example.com):");
            string email;
            while(string.IsNullOrEmpty(email = Console.ReadLine()) || !Regex.IsMatch(email, emailPattern))
            {
                if(!Regex.IsMatch(emailPattern, email))
                {
                    Console.Clear();
                    Console.WriteLine("Enter the user email (example@example.com):");
                    Console.WriteLine("Email does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter the user email (example@example.com):");
                    Console.WriteLine("Invalid email, please try again.");
                }
            }
            Console.Clear();
            Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
            string password;
            while (string.IsNullOrEmpty(password = Console.ReadLine()) || !Regex.IsMatch(password, passcodePattern))
            {
                if (!Regex.IsMatch(passcodePattern, password))
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Password does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Invalid password, please try again.");
                }
            }

            Console.Clear();
            string header = "Select gender:";
            string[] genderOption = {"Male", "Female"};
            int choice = ArrowKeySelection(genderOption.ToList(), header);

            var user = new User
            {
                FName = fname,
                LName = lname,
                Email = email,
                Passcode = password,
                Gender = genderOption[choice]
            };
            userAccess.RegisterUser(user);
            Console.WriteLine($"User \"{user.FName} {user.LName}\" added successfully.");
            ShowLoginMenu();
        }

        private static void AddAdmin()
        {
            Console.Clear();
            Console.Write("Enter the first name: ");
            string fname = Console.ReadLine();
            Console.Write("\nEnter the last name: ");
            string lname = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Enter the admin email (example@example.com):");
            string email;
            while (string.IsNullOrEmpty(email = Console.ReadLine()) || !Regex.IsMatch(email, emailPattern))
            {
                if (!Regex.IsMatch(emailPattern, email))
                {
                    Console.Clear();
                    Console.WriteLine("Enter the admin email (example@example.com):");
                    Console.WriteLine("Email does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter the admin email (example@example.com):");
                    Console.WriteLine("Invalid email, please try again.");
                }
            }
            Console.Clear();
            Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
            string password;
            while (string.IsNullOrEmpty(password = Console.ReadLine()) || !Regex.IsMatch(password, passcodePattern))
            {
                if (!Regex.IsMatch(passcodePattern, password))
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Password does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Invalid password, please try again.");
                }
            }

            var admin = new Admin
            {
                AdminFname = fname,
                AdminLname = lname,
                AdminEmail = email,
                AdminPasscode = password,
                MasterAdminId = 1
            };
            adminAccess.RegisterAdmin(admin);
            Console.WriteLine($"Admin \"{admin.AdminFname} {admin.AdminLname}\" added successfully.");
            ShowLoginMenu();
        }

        private static void InitialSetup()
        {
            Console.WriteLine("Welcome, please complete the initial setup to use the system.");
            Console.WriteLine("Initializing Master Admin:");
            Console.Write("\nEnter the first name: ");
            string fname = Console.ReadLine();
            Console.Write("\nEnter the last name: ");
            string lname = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Enter the master admin email (example@example.com):");
            string email;
            while (string.IsNullOrEmpty(email = Console.ReadLine()) || !Regex.IsMatch(email, emailPattern))
            {
                if (!Regex.IsMatch(emailPattern, email))
                {
                    Console.Clear();
                    Console.WriteLine("Enter the admin email (example@example.com):");
                    Console.WriteLine("Email does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter the admin email (example@example.com):");
                    Console.WriteLine("Invalid email, please try again.");
                }
            }
            Console.Clear();
            Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
            string password;
            while (string.IsNullOrEmpty(password = Console.ReadLine()) || !Regex.IsMatch(password, passcodePattern))
            {
                if (!Regex.IsMatch(passcodePattern, password))
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Password does not match the above pattern, please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Enter new password (8 characters, atleast 1 letter and 1 number):");
                    Console.WriteLine("Invalid password, please try again.");
                }
            }

            var admin = new Admin
            {
                AdminFname = fname,
                AdminLname = lname,
                AdminEmail = email,
                AdminPasscode = password,
                MasterAdminId = null
            };
            adminAccess.RegisterAdmin(admin);
            Console.WriteLine($"Master Admin \"{admin.AdminFname} {admin.AdminLname}\" added successfully.");

            Console.WriteLine("Initializing testing data.\nPress any key to continue...");
            Console.ReadKey();

            adminAccess.RegisterAdmin(new Admin
            {
                AdminFname = "Alice",
                AdminLname = "Smith",
                AdminEmail = "alice.smith@example.com",
                AdminPasscode = "Password123",
                MasterAdminId = 1
            });

            adminAccess.RegisterAdmin(new Admin
            {
                AdminFname = "Bob",
                AdminLname = "Johnson",
                AdminEmail = "bob.johnson@example.com",
                AdminPasscode = "SecurePass1",
                MasterAdminId = 1
            });

            adminAccess.RegisterAdmin(new Admin
            {
                AdminFname = "Clara",
                AdminLname = "Doe",
                AdminEmail = "clara.doe@example.com",
                AdminPasscode = "ClaraPass99",
                MasterAdminId = 1
            });

            adminAccess.RegisterAdmin(new Admin
            {
                AdminFname = "Daniel",
                AdminLname = "Brown",
                AdminEmail = "daniel.brown@example.com",
                AdminPasscode = "DanBrown!23",
                MasterAdminId = 1
            });

            adminAccess.RegisterAdmin(new Admin
            {
                AdminFname = "Eva",
                AdminLname = "Martinez",
                AdminEmail = "eva.martinez@example.com",
                AdminPasscode = "EvaMartinez1",
                MasterAdminId = 1
            });

            // Insert Users
            userAccess.RegisterUser(new User
            {
                FName = "David",
                LName = "Brown",
                Gender = "Male",
                Email = "david.brown@example.com",
                Passcode = "DavidPass9"
            });

            userAccess.RegisterUser(new User
            {
                FName = "Emma",
                LName = "Wilson",
                Gender = "Female",
                Email = "emma.wilson@example.com",
                Passcode = "EmmaPass123"
            });

            userAccess.RegisterUser(new User
            {
                FName = "Frank",
                LName = "Taylor",
                Gender = "Male",
                Email = "frank.taylor@example.com",
                Passcode = "Taylor1234"
            });

            userAccess.RegisterUser(new User
            {
                FName = "Grace",
                LName = "Harris",
                Gender = "Female",
                Email = "grace.harris@example.com",
                Passcode = "Grace!998"
            });

            userAccess.RegisterUser(new User
            {
                FName = "Henry",
                LName = "Clark",
                Gender = "Male",
                Email = "henry.clark@example.com",
                Passcode = "HenryC!654"
            });

            // Insert Categories
            adminAccess.AddCategory(new Category
            {
                CatName = "Programming",
                NumOfBooks = 0
            });

            adminAccess.AddCategory(new Category
            {
                CatName = "Databases",
                NumOfBooks = 0
            });

            adminAccess.AddCategory(new Category
            {
                CatName = "Web Development",
                NumOfBooks = 0
            });

            adminAccess.AddCategory(new Category
            {
                CatName = "Machine Learning",
                NumOfBooks = 0
            });

            adminAccess.AddCategory(new Category
            {
                CatName = "Mobile Development",
                NumOfBooks = 0
            });

            // Insert Books
            adminAccess.AddBook(new Book
            {
                BookName = "Introduction to C#",
                AuthorName = "Jane Doe",
                TotalCopies = 5,
                BorrowedCopies = 2,
                BorrowPeriod = 14,
                CopyPrice = 39.99m,
                CatId = 1 // Programming category
            });

            adminAccess.AddBook(new Book
            {
                BookName = "Mastering EF Core",
                AuthorName = "John Smith",
                TotalCopies = 3,
                BorrowedCopies = 1,
                BorrowPeriod = 21,
                CopyPrice = 49.99m,
                CatId = 2 // Databases category
            });

            adminAccess.AddBook(new Book
            {
                BookName = "Learning LINQ",
                AuthorName = "Emily White",
                TotalCopies = 4,
                BorrowedCopies = 0,
                BorrowPeriod = 14,
                CopyPrice = 29.99m,
                CatId = 1 // Programming category
            });

            adminAccess.AddBook(new Book
            {
                BookName = "Advanced SQL Queries",
                AuthorName = "Daniel Green",
                TotalCopies = 2,
                BorrowedCopies = 1,
                BorrowPeriod = 30,
                CopyPrice = 59.99m,
                CatId = 2 // Databases category
            });

            adminAccess.AddBook(new Book
            {
                BookName = "Entity Framework Essentials",
                AuthorName = "Sophia Brown",
                TotalCopies = 6,
                BorrowedCopies = 3,
                BorrowPeriod = 20,
                CopyPrice = 34.99m,
                CatId = 1 // Programming category
            });

            // Insert Borrows
            adminAccess.AddBorrow(new Borrow
            {
                BorrowDate = DateTime.Now.AddDays(-10),
                ReturnDate = DateTime.Now.AddDays(4),
                ActualReturnDate = null,
                IsReturned = false,
                Rating = 5,
                UserId = 1, // David
                BookId = 1  // Introduction to C#
            });

            adminAccess.AddBorrow(new Borrow
            {
                BorrowDate = DateTime.Now.AddDays(-20),
                ReturnDate = DateTime.Now.AddDays(-5),
                ActualReturnDate = DateTime.Now.AddDays(-4),
                IsReturned = true,
                Rating = 4,
                UserId = 2, // Emma
                BookId = 2  // Mastering EF Core
            });

            adminAccess.AddBorrow(new Borrow
            {
                BorrowDate = DateTime.Now.AddDays(-3),
                ReturnDate = DateTime.Now.AddDays(11),
                ActualReturnDate = null,
                IsReturned = false,
                Rating = null,
                UserId = 3, // Frank
                BookId = 3  // Learning LINQ
            });

            adminAccess.AddBorrow(new Borrow
            {
                BorrowDate = DateTime.Now.AddDays(-15),
                ReturnDate = DateTime.Now.AddDays(-1),
                ActualReturnDate = DateTime.Now,
                IsReturned = true,
                Rating = 5,
                UserId = 4, // Grace
                BookId = 4  // Advanced SQL Queries
            });

            adminAccess.AddBorrow(new Borrow
            {
                BorrowDate = DateTime.Now.AddDays(-25),
                ReturnDate = DateTime.Now.AddDays(-10),
                ActualReturnDate = DateTime.Now.AddDays(-9),
                IsReturned = true,
                Rating = 3,
                UserId = 5, // Henry
                BookId = 5  // Entity Framework Essentials
            });

            Console.WriteLine("Initial setup completed!\nPress any key to continue...");
            Console.ReadKey();
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

                if (isMasterAdmin)
                {
                    Console.WriteLine("Master Admin Authorized!");
                    ShowMasterAdminMenu(admin);
                }
                else
                {
                    Console.WriteLine("Login successful!");
                    ShowAdminMenu(admin);
                }
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
            int choice = -1;
            string header = "Admin Menu:";
            string[] options = { "Add Book", "View All Books", "View Users", "Manage Categories", "Search for Books", "Logout" };

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
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
            while (choice != 5);
        }

        private static void ShowMasterAdminMenu(Admin admin)
        {
            int choice = -1;
            string header = "Master Admin Menu:";
            string[] options = { "Add Book", "View All Books", "View Users", "Manage Categories", "Search for Books", "Logout" };

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
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
            while (choice != 5);
        }

        private static void ShowUserMenu(User user)
        {
            int choice = -1;
            string header = "User Menu:";
            string[] options = {"Browse Books", "Search for Books", "View Borrowed Books", "Edit User Info", "Logout"};
            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
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
            while (choice != 4);
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
            int selectedCategoryIndex = ArrowKeySelection(categories.Select(c => c.CatName).ToList(), header);
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
            int choice = -1;
            string header = "Select an option:";
            string[] options = {"Add category", "Change category name", "Delete category", "Exit category management"};

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                switch (choice)
                {
                    case 0:
                        break;

                    case 1:
                        break;

                    case 2:
                        break;

                    case 3:
                        break;
                    default:
                        break;
                }
            }
            while (choice != 3);
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