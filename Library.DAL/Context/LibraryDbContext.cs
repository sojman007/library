using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookHistory> BookHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(x => x.Author);
            modelBuilder.Entity<Book>()
                .HasIndex(x => x.Title);
            modelBuilder.Entity<Book>()
                .HasIndex(x => x.ISBN)
                .IsUnique();

            modelBuilder.Entity<BookHistory>()
                .HasIndex(x => x.BookId);
            modelBuilder.Entity<BookHistory>()
                .HasIndex(x => x.LenderId);
        }
    }
}
