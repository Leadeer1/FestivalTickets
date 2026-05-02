using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.InfrastructureLayer.EF;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.InfrastructureLayer.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.FestivalTickets.Core.InfrastructureLayer
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Rejestruje Infrastructure Layer (SQL Server).
        /// Wywołaj w Program.cs: services.AddInfrastructure(configuration)
        ///
        /// Wymagany connection string w appsettings.json:
        ///   "ConnectionStrings": { "FestivalDb": "Server=...;Database=FestivalTicketsDb;..." }
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FestivalDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("FestivalDb"),
                    sql => sql.MigrationsAssembly(
                        typeof(FestivalDbContext).Assembly.GetName().Name)));

            RegisterCommonServices(services);
            return services;
        }

        /// <summary>
        /// Wersja z bazą In-Memory – do testów lokalnych bez SQL Servera.
        /// Wywołaj w Program.cs: services.AddInfrastructureInMemory()
        /// </summary>
        public static IServiceCollection AddInfrastructureInMemory(
            this IServiceCollection services)
        {
            services.AddDbContext<FestivalDbContext>(options =>
                options.UseInMemoryDatabase("FestivalTicketsTestDb"));

            RegisterCommonServices(services);
            return services;
        }

        private static void RegisterCommonServices(IServiceCollection services)
        {
            // Unit of Work (zawiera repozytoria)
            services.AddScoped<IFestivalUnitOfWork, FestivalUnitOfWork>();

            // Domain Event Publisher z SharedKernel
            services.AddScoped<IDomainEventPublisher, SimpleEventPublisher>();

            // Notification Service
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
