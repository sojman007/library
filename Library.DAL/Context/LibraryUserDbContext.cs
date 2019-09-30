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
    public class LibraryUserDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LibraryUserDbContext(DbContextOptions<LibraryUserDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

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



        public override int SaveChanges()
        {
            this.OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
        

        private void OnBeforeSaving()
        {
            var entries = this.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (!(entry.Entity is BaseEntity coreEntity)) continue;

                var userId = string.Empty;
                if (this._httpContextAccessor != null && this._httpContextAccessor.HttpContext != null)
                    userId = this._httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;

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
