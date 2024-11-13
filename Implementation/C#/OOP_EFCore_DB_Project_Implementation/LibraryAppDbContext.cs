using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OOP_EFCore_DB_Project_Implementation.Models;

namespace OOP_EFCore_DB_Project_Implementation
{
    public class LibraryAppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(" Data Source=(local); Initial Catalog=LibraryORM_Project; Integrated Security=true; TrustServerCertificate=True ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                        .HasIndex(e => e.AdminEmail)
                        .IsUnique();

            modelBuilder.Entity<User>()
                        .HasIndex(e => e.Email)
                        .IsUnique();

            modelBuilder.Entity<Category>()
                        .HasIndex(e => e.CatName)
                        .IsUnique();

            modelBuilder.Entity<Book>()
                        .HasIndex(e => e.BookName)
                        .IsUnique();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
    }
}
