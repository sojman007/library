using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Context
{
    public class LibraryDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options, IHttpContextAccessor httpContextAccessor) 
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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

        

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void OnBeforeSaving()
        {
            var entries = this.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (!(entry.Entity is BaseEntity coreEntity)) continue;

                var userId = string.Empty;
                if (this._httpContextAccessor != null && this._httpContextAccessor.HttpContext != null)
                    userId = this._httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                switch (entry.State)
                {
                    case EntityState.Added:
                        coreEntity.CreatedOn = DateTime.Now;
                        coreEntity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        coreEntity.UpdatedOn = DateTime.Now;
                        coreEntity.UpdatedBy = userId;
                        break;
                }
            }
        }
    }
}
