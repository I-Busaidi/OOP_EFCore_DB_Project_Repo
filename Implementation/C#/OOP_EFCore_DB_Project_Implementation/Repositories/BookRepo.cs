﻿using Microsoft.EntityFrameworkCore;
using OOP_EFCore_DB_Project_Implementation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_EFCore_DB_Project_Implementation.Repositories
{
    public class BookRepo : IObjectsRepo<Book>
    {
        private readonly LibraryAppDbContext _context;
        public BookRepo(LibraryAppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }
        public Book GetByName(string name)
        {
            return _context.Books.FirstOrDefault(book => book.BookName == name);
        }
        public void Insert(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void UpdateByName(Book book, string name)
        {
            var bookToUpdate = GetByName(name);
            if (bookToUpdate != null)
            {
                try
                {
                    bookToUpdate.BookName = book.BookName;
                    bookToUpdate.AuthorName = book.AuthorName;
                    bookToUpdate.BorrowPeriod = book.BorrowPeriod;
                    bookToUpdate.BorrowedCopies = book.BorrowedCopies;
                    bookToUpdate.CopyPrice = book.CopyPrice;
                    bookToUpdate.CatId = book.CatId;
                    bookToUpdate.TotalCopies = book.TotalCopies;
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public void DeleteById(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                try
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public decimal GetTotalPrice()
        {
            return _context.Books.Sum(book => book.CopyPrice * book.TotalCopies);
        }
        public decimal GetMaxPrice()
        {
            return _context.Books.Max(book => book.CopyPrice);
        }
        public int GetTotalBorrowedBooks()
        {
            return _context.Borrows.Count(b => b.IsReturned == false);
        }
        public int GetTotalBooksPerCategoryName(string name)
        {
            return _context.Books.Count(book => book.Category.CatName == name);
        }

        public bool IsBookBorrowed(int id)
        {
            return _context.Borrows.Any(b => b.BookId == id && !b.IsReturned);
        }

        public IEnumerable<Book> GetBooksWithBorrows()
        {
            return _context.Books.Include(b => b.Borrows);
        }
    }
}
