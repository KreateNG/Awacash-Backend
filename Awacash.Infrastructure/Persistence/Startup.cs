using Awacash.Domain.Common;
using Awacash.Domain.Interfaces;
using Awacash.Infrastructure.Persistence.Context;
using Awacash.Infrastructure.Persistence.Repository;
using Awacash.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Persistence
{
    internal static class Startup
    {
        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            // TODO: there must be a cleaner way to do IOptions validation...
            var rootConnectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(rootConnectionString))
            {
                throw new InvalidOperationException("DB ConnectionString is not configured.");
            }


            return services
                .Configure<DatabaseSettings>(config.GetSection("DefaultConnection"))

                .AddDbContext<ApplicationDbContext>(m => m.UseDatabase(rootConnectionString))
                
                .AddRepositories();
        }

        internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var assemblyName = typeof(ApplicationDbContext).AssemblyQualifiedName;
            return builder.UseSqlServer(connectionString);
        }


        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Add Repositories
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(EfRepository<,>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            foreach (var aggregateRootType in
                typeof(IAggregateRoot).Assembly.GetExportedTypes()
                    .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                    .ToList())
            {


            }

            return services;
        }
    }
}
