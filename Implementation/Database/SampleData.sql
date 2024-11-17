use LibraryORM_Project
------------------------ADMINS-------------------------
-- Sample 1
INSERT INTO Admins (AdminFname, AdminLname, AdminEmail, AdminPasscode, MasterAdminId)
VALUES ('Alice', 'Smith', 'alice.smith@example.com', 'Password123', NULL);

-- Sample 2
INSERT INTO Admins (AdminFname, AdminLname, AdminEmail, AdminPasscode, MasterAdminId)
VALUES ('Bob', 'Johnson', 'bob.johnson@example.com', 'SecurePass1', 1);

-- Sample 3
INSERT INTO Admins (AdminFname, AdminLname, AdminEmail, AdminPasscode, MasterAdminId)
VALUES ('Clara', 'Doe', 'clara.doe@example.com', 'ClaraPass99', 1);

-- Sample 4
INSERT INTO Admins (AdminFname, AdminLname, AdminEmail, AdminPasscode, MasterAdminId)
VALUES ('Daniel', 'Brown', 'daniel.brown@example.com', 'DanBrown!23', NULL);

-- Sample 5
INSERT INTO Admins (AdminFname, AdminLname, AdminEmail, AdminPasscode, MasterAdminId)
VALUES ('Eva', 'Martinez', 'eva.martinez@example.com', 'EvaMartinez1', 2);

---------------------------USERS----------------------
-- Sample 1
INSERT INTO [Users] (FName, LName, Gender, Email, Passcode)
VALUES ('David', 'Brown', 'Male', 'david.brown@example.com', 'DavidPass9');

-- Sample 2
INSERT INTO [Users] (FName, LName, Gender, Email, Passcode)
VALUES ('Emma', 'Wilson', 'Female', 'emma.wilson@example.com', 'EmmaPass123');

-- Sample 3
INSERT INTO [Users] (FName, LName, Gender, Email, Passcode)
VALUES ('Frank', 'Taylor', 'Male', 'frank.taylor@example.com', 'Taylor1234');

-- Sample 4
INSERT INTO [Users] (FName, LName, Gender, Email, Passcode)
VALUES ('Grace', 'Harris', 'Female', 'grace.harris@example.com', 'Grace!998');

-- Sample 5
INSERT INTO [Users] (FName, LName, Gender, Email, Passcode)
VALUES ('Henry', 'Clark', 'Male', 'henry.clark@example.com', 'HenryC!654');

------------------------CATEGORIES-------------------------
-- Sample 1
INSERT INTO Categories(CatName, NumOfBooks)
VALUES ('Programming', 2);

-- Sample 2
INSERT INTO Categories (CatName, NumOfBooks)
VALUES ('Databases', 1);

-- Sample 3
INSERT INTO Categories (CatName, NumOfBooks)
VALUES ('Web Development', 3);

-- Sample 4
INSERT INTO Categories (CatName, NumOfBooks)
VALUES ('Machine Learning', 4);

-- Sample 5
INSERT INTO Categories (CatName, NumOfBooks)
VALUES ('Mobile Development', 2);

-----------------------BOOKS--------------------------
-- Sample 1
INSERT INTO Books (BookName, AuthorName, TotalCopies, BorrowedCopies, BorrowPeriod, CopyPrice, CatId)
VALUES ('Introduction to C#', 'Jane Doe', 5, 2, 14, 39.99, 1);

-- Sample 2
INSERT INTO Books (BookName, AuthorName, TotalCopies, BorrowedCopies, BorrowPeriod, CopyPrice, CatId)
VALUES ('Mastering EF Core', 'John Smith', 3, 1, 21, 49.99, 2);

-- Sample 3
INSERT INTO Books (BookName, AuthorName, TotalCopies, BorrowedCopies, BorrowPeriod, CopyPrice, CatId)
VALUES ('Learning LINQ', 'Emily White', 4, 0, 14, 29.99, 1);

-- Sample 4
INSERT INTO Books (BookName, AuthorName, TotalCopies, BorrowedCopies, BorrowPeriod, CopyPrice, CatId)
VALUES ('Advanced SQL Queries', 'Daniel Green', 2, 1, 30, 59.99, 2);

-- Sample 5
INSERT INTO Books (BookName, AuthorName, TotalCopies, BorrowedCopies, BorrowPeriod, CopyPrice, CatId)
VALUES ('Entity Framework Essentials', 'Sophia Brown', 6, 3, 20, 34.99, 1);

------------------------BORROWS-------------------------
-- Sample 1
INSERT INTO Borrows (BorrowDate, ReturnDate, ActualReturnDate, IsReturned, Rating, UserId, BookId)
VALUES (GETDATE() - 10, GETDATE() + 4, NULL, 0, 5, 1, 1);

-- Sample 2
INSERT INTO Borrows (BorrowDate, ReturnDate, ActualReturnDate, IsReturned, Rating, UserId, BookId)
VALUES (GETDATE() - 20, GETDATE() - 5, GETDATE() - 4, 1, 4, 2, 2);

-- Sample 3
INSERT INTO Borrows (BorrowDate, ReturnDate, ActualReturnDate, IsReturned, Rating, UserId, BookId)
VALUES (GETDATE() - 3, GETDATE() + 11, NULL, 0, NULL, 3, 3);

-- Sample 4
INSERT INTO Borrows (BorrowDate, ReturnDate, ActualReturnDate, IsReturned, Rating, UserId, BookId)
VALUES (GETDATE() - 15, GETDATE() - 1, GETDATE(), 1, 5, 4, 4);

-- Sample 5
INSERT INTO Borrows (BorrowDate, ReturnDate, ActualReturnDate, IsReturned, Rating, UserId, BookId)
VALUES (GETDATE() - 25, GETDATE() - 10, GETDATE() - 9, 1, 3, 5, 5);
