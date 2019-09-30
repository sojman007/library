using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.BLL.Services;
using Library.BLL.Services.Interfaces;
using Library.DAL;
using Library.DAL.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.BLL
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterRepository(configuration);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
        }
    }
}
