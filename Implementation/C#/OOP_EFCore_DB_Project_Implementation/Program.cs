using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

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


        //========== UTILITY FUNCTIONS ==========//
        private static void Logout()
        {
            isUserLoggedIn = false;
            isAdminLoggedIn = false;
            currentUserId = null;
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
                BorrowedCopies = 1,
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
                BorrowedCopies = 1,
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
                BorrowedCopies = 1,
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
            else
            {
                return;
            }
        }
        private static int ArrowKeySelection(List<string> options, string head)
        {
            int selectedIndex = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(head + "\n\n");
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
                Console.WriteLine("\n\nUse arrow keys to select. or 'Esc' to exit menu");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Count - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex < options.Count - 1) ? selectedIndex + 1 : 0;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    return selectedIndex;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    return -1;
                }
            }
        }


        //========== USER RELATED OPERATIONS ==========//
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
            if (choice == -1)
            {
                Console.WriteLine("Returning to login menu.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
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
            }
            ShowLoginMenu();
        }
        private static void ViewAllUsers()
        {
            Console.Clear();
            var users = adminAccess.ViewAllUsers();
            int counter = 1;
            string border = new string('=', 60);
            StringBuilder sb = new StringBuilder();
            foreach (var user in users)
            {
                sb.AppendLine($"{counter, -3} | {user.FName+" "+user.LName, -25} | {user.Email}");
                counter++;
            }
            Console.WriteLine($"{"No.",-3} | {"Name",-25} | Email");
            Console.WriteLine(border);
            Console.WriteLine(sb.ToString());
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
        private static void ShowUserMenu(User user)
        {
            string header;
            int choice;
            string[] options;
            bool borrowPending = true;
            var userBorrows = userAccess.GetCurrentBorrows(user.UserId);
            if (userBorrows.Any(b => b.ReturnDate < DateTime.Now && b.IsReturned == false))
            {
                do
                {
                    choice = -1;
                    header = "User Menu (! Must return overdue books before accessing other services !):";
                    options = new string[] { "Return Book", "Edit User Info", "Logout" };
                    choice = ArrowKeySelection(options.ToList(), header);
                    if (choice == 0)
                    {
                        ReturnBook(user);
                        var userBorrow = userAccess.GetCurrentBorrows(user.UserId);
                        if (!userBorrows.Any(b => b.ReturnDate < DateTime.Now && b.IsReturned == false))
                        {
                            borrowPending = false;
                        }
                    }
                    else if (choice == 1)
                    {
                        EditUserInfo(user);
                    }
                    else
                    {
                        Logout();
                    }
                }
                while (borrowPending);
            }
            choice = -1;
            header = "User Menu:";
            options = new string[] { "Browse Books", "Search for Books", "View Borrowed Books", "Return Book", "Edit User Info", "Logout" };
            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                if (choice == 0)
                {
                    BrowseBooks();
                }
                else if (choice == 1)
                {
                    SearchBooks();
                }
                else if (choice == 2)
                {
                    ViewBorrowedBooks(user);
                }
                else if (choice == 3)
                {
                    ReturnBook(user);
                }
                else if (choice == 4)
                {
                    EditUserInfo(user);
                }
                else if (choice == 5)
                {
                    Logout();
                }
            }
            while (choice != 5);
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
            string header = $"Select an editing option for {user.FName + " " + user.LName}: ";
            string[] options = {"Edit Name","Edit Email","Edit Password", "Remove User","Exit"};
            int choice = -1;

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                switch (choice)
                {
                    case 0:
                        EditUserName(user);
                        break;

                    case 1:
                        EditUserEmail(user);
                        break;

                    case 2:
                        EditUserPasscode(user);
                        break;

                    case 3:
                        DeleteUser(user);
                        break;

                    case 4:
                    case -1:
                        choice = 4;
                        break;

                    default:
                        break;
                }
            }
            while (choice != 4);

        }
        private static void EditUserName(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the new first name for user:");
            string newFname;
            while(string.IsNullOrEmpty(newFname = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new first name for user:");
                Console.WriteLine("Invalid input, please try again.");
            }

            Console.WriteLine("Enter the new last name for user:");
            string newLname;
            while (string.IsNullOrEmpty(newLname = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new last name for user:");
                Console.WriteLine("Invalid input, please try again.");
            }

            userAccess.EditInfo(new User
            {
                UserId = user.UserId,
                FName = newFname,
                LName = newLname,
                Email = user.Email,
                Passcode = user.Passcode,
                Gender = user.Gender,
            });
        }
        private static void EditUserEmail(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the new email for user (example@example.com):");
            string email;
            while (string.IsNullOrEmpty(email = Console.ReadLine()) || !Regex.IsMatch(email, emailPattern))
            {
                Console.Clear();
                Console.WriteLine("Enter the new email for user (example@example.com):");
                if(!Regex.IsMatch(email, emailPattern))
                {
                    Console.WriteLine("Email does not match the required pattern, please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            userAccess.EditInfo(new User
            {
                UserId = user.UserId,
                FName = user.FName,
                LName = user.LName,
                Email = email,
                Passcode = user.Passcode,
                Gender = user.Gender,
            });
        }
        private static void EditUserPasscode(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the new password for user (8 characters, 1 letter and 1 number at least):");
            string password;
            while (string.IsNullOrEmpty(password = Console.ReadLine()) || !Regex.IsMatch(password, passcodePattern))
            {
                Console.Clear();
                Console.WriteLine("Enter the new password for user (8 characters, 1 letter and 1 number at least):");
                if (!Regex.IsMatch(password, passcodePattern))
                {
                    Console.WriteLine("Password does not match the required pattern, please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            userAccess.EditInfo(new User
            {
                UserId = user.UserId,
                FName = user.FName,
                LName = user.LName,
                Email = user.Email,
                Passcode = password,
                Gender = user.Gender,
            });
        }
        private static void DeleteUser(User user)
        {
            string header = $"Confirm removal of user: {user.FName + " " + user.LName}? ";
            string[] options = {"Yes","No"};
            int choice = ArrowKeySelection(options.ToList(), header);

            if (choice == 0)
            {
                adminAccess.RemoveUser(user.UserId);
            }
            else
            {
                Console.WriteLine("User removal has been cancelled.");
            }
        }
        private static void ManageUser()
        {
            string searchTerm;
            Console.Clear();
            Console.WriteLine("Enter the user name to manage: ");
            while(string.IsNullOrEmpty(searchTerm = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the user name to manage: ");
                Console.WriteLine("Invalid input, please try again.");
            }

            var usersList = adminAccess.GetUsersByName(searchTerm).ToList();
            if (usersList.Count() == 0)
            {
                Console.WriteLine("No user with this name found");
            }
            else if (usersList.Count() == 1)
            {
                EditUserInfo(usersList[0]);
            }
            else
            {
                string header = "Select a user from the list that matched the name: ";
                int selectedIndex = ArrowKeySelection(usersList.Select(u => $"{u.FName + " " + u.LName, -30} | Email: {u.Email}").ToList(), header);
                if (selectedIndex != -1)
                {
                    var selectedUser = usersList[selectedIndex];
                    EditUserInfo(selectedUser);
                }
            }
        }


        //========== ADMIN RELATED OPERATIONS ==========//
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
        private static void AdminLogin()
        {
            //Testing purpose only, must remove before implementation or I will get fired.
            var mAdmin = adminAccess.GetMasterAdminSecets();
            string hintEmail = mAdmin.AdminEmail, hintPassword = mAdmin.AdminPasscode;
            Console.Clear();
            Console.WriteLine($"Enter Admin Email (hint: Master admin email: {hintEmail} ):");
            string email = Console.ReadLine();
            Console.WriteLine($"\nEnter Admin Password (hint: Master admin password: {hintPassword} ):");
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
        private static void ShowAdminMenu(Admin admin)
        {
            int choice = -1;
            string header = "Admin Menu:";
            string[] options = { "Add Book", "View All Books", "View Users", "Manage Categories", "Search for Books", "Manage Users", "Manage Books", "Logout" };

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                if (choice == 0)
                {
                    AddBook();
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
                    SearchBooks();
                }
                else if(choice == 5)
                {
                    ManageUser();
                }
                else if(choice == 6)
                {
                    ManageBook();
                }
                else
                {
                    Logout();
                }
            }
            while (choice != 5);
        }
        private static void ShowMasterAdminMenu(Admin admin)
        {
            int choice = -1;
            string header = "Master Admin Menu:";
            string[] options = { "Add Book", "View All Books", "View Users", "Manage Categories", "Search for Books", "Manage Users", "Manage Admins", "Manage Books", "Logout" };

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                if (choice == 0)
                {
                    AddBook();
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
                    SearchBooks();
                }
                else if(choice == 5)
                {
                    ManageUser();
                }
                else if(choice == 6)
                {
                    ManageAdmin();
                }
                else if( choice == 7)
                {
                    ManageBook();
                }
                else
                {
                    Logout();
                    return;
                }
            }
            while (choice != 8 || choice != -1);
        }
        private static void DeleteAdmin(Admin admin)
        {
            string header = $"Confirm removal of admin: {admin.AdminFname + " " + admin.AdminLname}?";
            string[] options = { "Yes", "No" };
            int choice = ArrowKeySelection(options.ToList(), header);

            if (choice == 0)
            {
                adminAccess.RemoveAdmin(admin.AdminId);
            }
            else
            {
                Console.WriteLine("Admin removal has been cancelled.");
            }
        }
        private static void EditAdminInfo(Admin admin)
        {
            string header = "Select an editing option: ";
            string[] options = { "Edit Name", "Edit Email", "Edit Password", "Remove Admin", "Exit" };
            int choice = -1;

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                switch (choice)
                {
                    case 0:
                        EditAdminName(admin);
                        break;

                    case 1:
                        EditAdminEmail(admin);
                        break;

                    case 2:
                        EditAdminPasscode(admin);
                        break;

                    case 3:
                        DeleteAdmin(admin);
                        break;

                    case 4:
                    case -1:
                        choice = 4;
                        break;

                    default:
                        break;
                }
            }
            while (choice != 4);
        }
        private static void EditAdminName(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Enter the new first name for admin:");
            string newFname;
            while (string.IsNullOrEmpty(newFname = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new first name for admin:");
                Console.WriteLine("Invalid input, please try again.");
            }

            Console.WriteLine("Enter the new last name for admin:");
            string newLname;
            while (string.IsNullOrEmpty(newLname = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new last name for admin:");
                Console.WriteLine("Invalid input, please try again.");
            }

            adminAccess.UpdateAdmin(new Admin
            {
                AdminEmail = admin.AdminEmail,
                AdminPasscode = admin.AdminPasscode,
                MasterAdminId = admin.MasterAdminId,
                AdminFname = newFname,
                AdminLname = newLname
            });
        }
        private static void EditAdminEmail(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Enter the new email for admin (example@example.com):");
            string email;
            while (string.IsNullOrEmpty(email = Console.ReadLine()) || !Regex.IsMatch(email, emailPattern))
            {
                Console.Clear();
                Console.WriteLine("Enter the new email for admin (example@example.com):");
                if (!Regex.IsMatch(email, emailPattern))
                {
                    Console.WriteLine("Email does not match the required pattern, please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            adminAccess.UpdateAdmin(new Admin
            {
                AdminEmail = email,
                AdminPasscode = admin.AdminPasscode,
                MasterAdminId = admin.MasterAdminId,
                AdminFname = admin.AdminFname,
                AdminLname = admin.AdminLname
            });
        }
        private static void EditAdminPasscode(Admin admin)
        {
            Console.Clear();
            Console.WriteLine("Enter the new password for admin (8 characters, 1 letter and 1 number at least):");
            string password;
            while (string.IsNullOrEmpty(password = Console.ReadLine()) || !Regex.IsMatch(password, passcodePattern))
            {
                Console.Clear();
                Console.WriteLine("Enter the new password for admin (8 characters, 1 letter and 1 number at least):");
                if (!Regex.IsMatch(password, passcodePattern))
                {
                    Console.WriteLine("Password does not match the required pattern, please try again.");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            adminAccess.UpdateAdmin(new Admin
            {
                AdminEmail = admin.AdminEmail,
                AdminPasscode = password,
                MasterAdminId = admin.MasterAdminId,
                AdminFname = admin.AdminFname,
                AdminLname = admin.AdminLname
            });
        }
        private static void ManageAdmin()
        {
            string searchTerm;
            Console.Clear();
            Console.WriteLine("Enter the admin name to manage: ");
            while (string.IsNullOrEmpty(searchTerm = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the admin name to manage: ");
                Console.WriteLine("Invalid input, please try again.");
            }

            var adminsList = adminAccess.GetAdminsByName(searchTerm).ToList();
            if (adminsList.Count() == 0)
            {
                Console.WriteLine("No admin with this name found");
            }
            else if (adminsList.Count() == 1)
            {
                EditAdminInfo(adminsList[0]);
            }
            else
            {
                string header = "Select an admin from the list that matched the name: ";
                int selectedIndex = ArrowKeySelection(adminsList.Select(a => $"{a.AdminFname + " " + a.AdminLname,-30} | Email: {a.AdminEmail}").ToList(), header);
                var selectedAdmin = adminsList[selectedIndex];
                EditAdminInfo(selectedAdmin);
            }
        }


        //========== BOOK RELATED OPERATIONS ==========//
        private static void BrowseBooks()
        {
            var books = userAccess.ViewAllBooks();
            if (books != null && books.Any())
            {
                var bookList = books.ToList();
                Console.Clear();
                string header = "Select a Book to Borrow:";

                int selectedIndex = ArrowKeySelection(bookList.Select(b => $"{b.BookName, -30} | by {b.AuthorName, -20} | Available copies: {b.TotalCopies - b.BorrowedCopies, -1}").ToList(), header);
                if (selectedIndex == -1)
                {
                    Console.WriteLine("Book borrowing cancelled.\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    var selectedBook = bookList[selectedIndex];
                    userAccess.BorrowBook(currentUserId.Value, selectedBook.BookId);
                    ShowRecommendations(selectedBook);
                }
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
            string header = "Recommended books based on your selection:";

            var random = new Random();
            var randomBooks = recommendedBooks.OrderBy(x => random.Next()).Take(5).ToList();
            int selectedBookIndex = ArrowKeySelection(randomBooks.Select(b => $"{b.BookName} by {b.AuthorName}").ToList(), header);

            if (selectedBookIndex >= 0 && selectedBookIndex < randomBooks.Count)
            {
                var selectedBook = randomBooks[selectedBookIndex];
                userAccess.BorrowBook(currentUserId.Value, selectedBook.BookId);
            }
            else
            {
                Console.WriteLine("Returning to menu.\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        private static void AddBook()
        {
            Console.Clear();
            Console.WriteLine("Enter Book Name:");
            string bookName = Console.ReadLine();
            Console.WriteLine("Enter Author Name:");
            string authorName = Console.ReadLine();

            var categories = adminAccess.ViewAllCategories();
            string header = "Select a category for the book:";
            int selectedCategoryIndex = ArrowKeySelection(categories.Select(c => c.CatName).ToList(), header);

            if (selectedCategoryIndex == -1)
            {
                Console.WriteLine("Book addition cancelled.\nPress any key to continue...");
                Console.ReadKey();
            }
            var selectedCategory = categories.ElementAt(selectedCategoryIndex);

            Console.Clear();
            Console.WriteLine("Enter the amount of available copies:");
            int copyAmount;
            while(!int.TryParse(Console.ReadLine(), out copyAmount) || copyAmount < 1)
            {
                Console.Clear();
                if (copyAmount < 1)
                {
                    Console.WriteLine("Enter the amount of available copies:");
                    Console.WriteLine("Amount must be more than 0, please try again.");
                }
                else
                {
                    Console.WriteLine("Enter the amount of available copies:");
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            Console.Clear();
            Console.WriteLine("Enter the maximum allowed borrowing period in days:");
            int borrowPeriod;
            while(!int.TryParse(Console.ReadLine(), out borrowPeriod) || borrowPeriod < 1)
            {
                Console.Clear();
                if (borrowPeriod < 1)
                {
                    Console.WriteLine("Enter the maximum allowed borrowing period in days:");
                    Console.WriteLine("Input must be more than 0, please try again.");
                }
                else
                {
                    Console.WriteLine("Enter the maximum allowed borrowing period in days:");
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            Console.Clear();
            Console.WriteLine($"Enter the price of each copy of the book \"{bookName}\":");
            decimal price;
            while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.Clear();
                if (price <= 0)
                {
                    Console.WriteLine($"Enter the price of each copy of the book \"{bookName}\":");
                    Console.WriteLine("price must be more than 0, please try again.");
                }
                else
                {
                    Console.WriteLine($"Enter the price of each copy of the book \"{bookName}\":");
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            var book = new Book
            {
                BookName = bookName,
                AuthorName = authorName,
                TotalCopies = copyAmount,
                BorrowPeriod = borrowPeriod,
                CopyPrice = price,
                CatId = selectedCategory.CatId
            };
            adminAccess.AddBook(book);
            Console.WriteLine($"\"{bookName}\" has been added successfully.");
        }
        private static void SearchBooks()
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
            int counter = 1;
            string border = new string('=', 83);
            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{counter, -3} | {book.BookName, -30} | {book.AuthorName, -25} | {book.TotalCopies - book.BorrowedCopies}");
                counter++;
            }
            Console.WriteLine($"{"No.",-3} | {"Book Name",-30} | {"Author Name", -25} | Available Copies");
            Console.WriteLine(border);
            Console.WriteLine(sb.ToString());
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        private static void EditBookInfo(Book book)
        {
            string header = "Select an editing option: ";
            string[] options = { "Edit Name", "Edit Author Name", "Edit Price", "Edit Borrow Period", "Add Copies", "Remove Book", "Exit" };
            int choice = -1;

            do
            {
                choice = ArrowKeySelection(options.ToList(), header);
                switch (choice)
                {
                    case 0:
                        EditBookName(book);
                        break;

                    case 1:
                        EditBookAuthorName(book);
                        break;

                    case 2:
                        EditBookPrice(book);
                        break;

                    case 3:
                        EditBorrowPeriod(book);
                        break;

                    case 4:
                        AddCopies(book);
                        break;

                    case 5:
                        DeleteBook(book);
                        break;

                    case 6:
                    case -1:
                        choice = 6;
                        break;

                    default:
                        break;
                }
            }
            while (choice != 6);
        }
        private static void EditBookName(Book book)
        {
            Console.Clear();
            Console.WriteLine("Enter the new name for the book: ");
            string bName;
            while(string.IsNullOrEmpty(bName = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new name for the book: ");
                Console.WriteLine("Invalid input, please try again.");
            }

            adminAccess.UpdateBook(book.BookName, new Book
            {
                BookName = bName,
                AuthorName = book.AuthorName,
                BorrowedCopies = book.BorrowedCopies,
                BorrowPeriod = book.BorrowPeriod,
                TotalCopies = book.TotalCopies,
                CopyPrice = book.CopyPrice,
                CatId = book.CatId,
            });
        }
        private static void EditBookAuthorName(Book book)
        {
            Console.Clear();
            Console.WriteLine("Enter the new author name for the book: ");
            string newAuthorName;
            while (string.IsNullOrEmpty(newAuthorName = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new author name for the book: ");
                Console.WriteLine("Invalid input, please try again.");
            }

            adminAccess.UpdateBook(book.BookName, new Book
            {
                BookName = book.BookName,
                AuthorName = newAuthorName,
                BorrowedCopies = book.BorrowedCopies,
                BorrowPeriod = book.BorrowPeriod,
                TotalCopies = book.TotalCopies,
                CopyPrice = book.CopyPrice,
                CatId = book.CatId,
            });
        }
        private static void EditBookPrice(Book book)
        {
            Console.Clear();
            Console.WriteLine("Enter the new price for the book: ");
            decimal newPrice;
            while (!decimal.TryParse(Console.ReadLine(), out newPrice) || newPrice <= 0)
            {
                Console.Clear();
                Console.WriteLine("Enter the new price for the book: ");
                if (newPrice <= 0)
                {
                    Console.WriteLine("Price must be greater than 0");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }
            adminAccess.UpdateBook(book.BookName, new Book
            {
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                BorrowedCopies = book.BorrowedCopies,
                BorrowPeriod = book.BorrowPeriod,
                TotalCopies = book.TotalCopies,
                CopyPrice = newPrice,
                CatId = book.CatId,
            });
        }
        private static void EditBorrowPeriod(Book book)
        {
            Console.Clear();
            Console.WriteLine("Enter the new allowed borrow period for the book: ");
            int newPeriod;
            while (!int.TryParse(Console.ReadLine(), out newPeriod) || newPeriod <= 0)
            {
                Console.Clear();
                Console.WriteLine("Enter the new allowed borrow period for the book: ");
                if (newPeriod <= 0)
                {
                    Console.WriteLine("Allowed borrow period must be greater than 0");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }
            adminAccess.UpdateBook(book.BookName, new Book
            {
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                BorrowedCopies = book.BorrowedCopies,
                BorrowPeriod = newPeriod,
                TotalCopies = book.TotalCopies,
                CopyPrice = book.CopyPrice,
                CatId = book.CatId,
            });
        }
        private static void AddCopies(Book book)
        {
            Console.Clear();
            Console.WriteLine("Enter the number of copies to add for the book: ");
            int addedCopies;
            while (!int.TryParse(Console.ReadLine(), out addedCopies) || addedCopies < 0)
            {
                Console.Clear();
                Console.WriteLine("Enter the number of copies to add for the book: ");
                Console.WriteLine("Invalid input, please try again.");
            }

            adminAccess.UpdateBook(book.BookName, new Book
            {
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                BorrowedCopies = book.BorrowedCopies,
                BorrowPeriod = book.BorrowPeriod,
                TotalCopies = book.TotalCopies + addedCopies,
                CopyPrice = book.CopyPrice,
                CatId = book.CatId,
            });
        }
        private static void DeleteBook(Book book)
        {
            string header = $"Confirm removal of book: {book.BookName}? ";
            string[] options = { "Yes", "No" };
            int choice = ArrowKeySelection(options.ToList(), header);

            if (choice == 0)
            {
                adminAccess.DeleteBook(book.BookName);
            }
            else
            {
                Console.WriteLine("Book removal has been cancelled.");
            }
        }
        private static void ReturnBook(User user)
        {
            var borrows = userAccess.GetCurrentBorrows(user.UserId);
            if (!borrows.Any())
            {
                Console.Clear() ;
                Console.WriteLine("No books currently borrowed by this user.\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                var borrowsList = borrows.ToList();
                string header = "Select a book to return: ";
                int selectedIndex = ArrowKeySelection(borrowsList.Select(b => $"{b.Book.BookName, -30} | Due Date: {b.ReturnDate, -30} | Days left: {(b.ReturnDate - DateTime.Now).Days}").ToList(), header);
                if (selectedIndex == -1)
                {
                    Console.WriteLine("Cancelling return.\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
                var selectedBorrow = borrowsList[selectedIndex];
                Console.WriteLine($"Rate the book \"{selectedBorrow.Book.BookName}\" out of 5: ");
                int rating = 0;
                while(!int.TryParse(Console.ReadLine(), out rating) || rating < 0 || rating > 5)
                {
                    Console.Clear();
                    Console.WriteLine($"Rate the book \"{selectedBorrow.Book.BookName}\" out of 5: ");
                    if (rating < 0 || rating > 5)
                    {
                        Console.WriteLine("Rating must be from 0 to 5.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again.");
                    }
                }
                userAccess.ReturnBook(user.UserId, selectedBorrow.BookId, rating);
                ShowRecommendations(selectedBorrow.Book);
            }
        }
        private static void ManageBook()
        {
            string searchTerm;
            Console.Clear();
            Console.WriteLine("Enter the book or author name to manage book(s): ");
            while (string.IsNullOrEmpty(searchTerm = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the book or author name to manage book(s): ");
                Console.WriteLine("Invalid input, please try again.");
            }

            var booksList = userAccess.SearchBooks(searchTerm).ToList();
            if (booksList.Count() == 0)
            {
                Console.WriteLine("No book or author with this name found");
            }
            else if (booksList.Count() == 1)
            {
                EditBookInfo(booksList[0]);
            }
            else
            {
                string header = "Select a book from the list that matched the name: ";
                int selectedIndex = ArrowKeySelection(booksList.Select(b => $"{b.BookName,-30} | By: {b.AuthorName}").ToList(), header);
                if (selectedIndex == -1)
                {
                    Console.WriteLine("Returning to menu.\nPress any key to continue...");
                    Console.ReadKey();
                    return;
                }
                var selectedBook = booksList[selectedIndex];
                EditBookInfo(selectedBook);
            }
        }


        //========== CATEGORY RELATED OPERATIONS ==========//
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
                        AddCategory();
                        break;

                    case 1:
                        UpdateCategory();
                        break;

                    case 2:
                        DeleteCategory();
                        break;

                    case 3:
                    case -1:
                        choice = 3;
                        break;
                    default:
                        break;
                }
            }
            while (choice != 3);
        }
        private static void AddCategory()
        {
            Console.Clear();
            Console.WriteLine("Enter the new category name:");
            string name;
            while(string.IsNullOrEmpty(name = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("Enter the new category name:");
                Console.WriteLine("Invalid input, please try again.");
            }

            adminAccess.AddCategory(new Category
            {
                CatName = name
            });

            Console.Clear();
            Console.WriteLine($"Category \"{name}\" has been added successfully.");
        }
        private static void UpdateCategory()
        {
            var categories = adminAccess.ViewAllCategories();
            string header = "Select a category to update:";
            int selectedCategoryIndex = ArrowKeySelection(categories.Select(c => c.CatName).ToList(), header);
            if (selectedCategoryIndex != -1)
            {
                string newName;
                Console.Clear();
                Console.WriteLine($"Enter the new name for category \"{categories.ToList()[selectedCategoryIndex].CatName}\"");
                while (string.IsNullOrEmpty(newName = Console.ReadLine()))
                {
                    Console.Clear();
                    Console.WriteLine($"Enter the new name for category \"{categories.ToList()[selectedCategoryIndex].CatName}\"");
                    Console.WriteLine("Invalid input, please try again.");
                }
                var UpdatedCategory = new Category
                {
                    CatName = newName,
                };

                adminAccess.UpdateCategory(UpdatedCategory, categories.ToList()[selectedCategoryIndex].CatName);
            }
        }
        private static void DeleteCategory()
        {
            var categories = adminAccess.ViewAllCategories();
            string header = "Select a category to delete:";
            int selectedCategoryIndex = ArrowKeySelection(categories.Select(c => c.CatName).ToList(), header);

            if (selectedCategoryIndex == -1)
            {
                Console.WriteLine("Cancelling category removal.\nPress any key to continue...");
                Console.ReadKey();
            }
            else
            {
                string headerConf = $"Confirm removal of category: {categories.ToList()[selectedCategoryIndex].CatName}? ";
                string[] options = { "Yes", "No" };
                int choice = ArrowKeySelection(options.ToList(), headerConf);

                if (choice == 0)
                {
                    adminAccess.DeleteCategory(categories.ToList()[selectedCategoryIndex].CatName);
                }
                else
                {
                    Console.WriteLine("Cancelling category removal.\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}