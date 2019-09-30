using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Context
{
    public class LibraryUserDbContext : DbContext
    {
        public LibraryUserDbContext(DbContextOptions<LibraryUserDbContext> options)
        : base(options)
        { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserTokens> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(x => x.PasswordHash);

            modelBuilder.Entity<UserTokens>()
                .HasIndex(x => x.UserId);
            modelBuilder.Entity<UserTokens>()
                .HasIndex(x => x.Token);
        }
    }
}
