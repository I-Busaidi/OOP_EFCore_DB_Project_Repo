using OOP_EFCore_DB_Project_Implementation.Models;
using OOP_EFCore_DB_Project_Implementation.Repositories;

namespace OOP_EFCore_DB_Project_Implementation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var dbContext = new LibraryAppDbContext();
            var adminRepository = new AdminRepo(dbContext);
            var userRepository = new UserRepo(dbContext);
            var bookRepository = new BookRepo(dbContext);
            var categoryRepository = new CategoryRepo(dbContext);
            var borrowRepository = new BorrowRepo(dbContext);

            var adminAccess = new AdminAccess(adminRepository, userRepository, bookRepository, categoryRepository, borrowRepository);
            var userAccess = new UserAccess(userRepository, bookRepository, borrowRepository, categoryRepository);

            Console.WriteLine("Welcome to the Library Management System");

            while (true)
            {
                Console.WriteLine("Are you an (1) Admin or (2) User? (Type 'exit' to exit)");
                string userType = Console.ReadLine();

                if (userType == "exit")
                    break;

                switch (userType)
                {
                    case "1":
                        AdminInteraction(adminAccess);
                        break;
                    case "2":
                        UserInteraction(userAccess);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void AdminInteraction(AdminAccess adminAccess)
        {
            Console.WriteLine("Admin Mode");

            Console.WriteLine("Enter your email:");
            var email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            var password = Console.ReadLine();

            var admin = adminAccess.LoginAdmin(email, password);

            if (admin != null)
            {
                Console.WriteLine("Logged in successfully!");

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Choose an action: (1) Add Book (2) Update Book (3) Delete Book (4) View All Books (5) Logout");
                    var action = Console.ReadLine();

                    switch (action)
                    {
                        case "1":
                            
                            break;
                        case "2":
                            
                            break;
                        case "3":
                            
                            break;
                        case "4":
                            
                            break;
                        case "5":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid email or password.");
            }
        }

        static void UserInteraction(UserAccess userAccess)
        {
            Console.WriteLine("User Mode");

            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();

            Console.WriteLine("Enter your passcode:");
            string passcode = Console.ReadLine();

            var user = userAccess.LoginUser(email, passcode);

            if (user != null)
            {
                Console.WriteLine("Logged in successfully!");

                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Choose an action: (1) View Books (2) Borrow Book (3) Return Book (4) Logout");
                    var action = Console.ReadLine();

                    switch (action)
                    {
                        case "1":
                            var allBooks = userAccess.ViewAllBooks();
                            Console.WriteLine("Books:");
                            foreach (var b in allBooks)
                            {
                                Console.WriteLine($"ID: {b.BookId}, Name: {b.BookName}, Category: {b.Category.CatName}, Price: {b.CopyPrice}, Available Copies: {b.TotalCopies - b.BorrowedCopies}");
                            }
                            break;
                        case "2":
                            Console.WriteLine("Enter the ID of the book to borrow:");
                            var bookIdToBorrow = int.Parse(Console.ReadLine());
                            userAccess.BorrowBook(user.UserId, bookIdToBorrow);
                            Console.WriteLine("Book borrowed successfully!");
                            break;
                        case "3":
                            Console.WriteLine("Enter the ID of the book to return:");
                            var bookIdToReturn = int.Parse(Console.ReadLine());
                            userAccess.ReturnBook(user.UserId, bookIdToReturn);
                            Console.WriteLine("Book returned successfully!");
                            break;
                        case "4":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid passcode.");
            }
        }
    }
}
