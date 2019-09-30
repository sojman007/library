using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.DAL.Context;
using Library.DAL.Repositories;
using Library.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DAL
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(x => {
                x.UseSqlServer(configuration.GetConnectionString("LibraryConnection"));
            });
            services.AddDbContext<LibraryUserDbContext>(x => {
                x.UseSqlServer(configuration.GetConnectionString("LibraryUserConnection"));
            });
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserTokenRepo, UserTokenRepo>();
            services.AddScoped<IBookRepo, BookRepo>();
            services.AddScoped<IBookHistoryRepo, BookHistoryRepo>();

            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IUserRoleRepo, UserRoleRepo>();
        }
    }
}
