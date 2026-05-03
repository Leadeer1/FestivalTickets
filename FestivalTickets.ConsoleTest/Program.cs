using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DDD.FestivalTickets.Core.InfrastructureLayer.EF;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.ApplicationLayer.Commands.Handlers;
using DDD.FestivalTickets.Core.ApplicationLayer.Queries.Handlers;
using DDD.FestivalTickets.Core.DomainModelLayer.Factories;
using DDD.FestivalTickets.Core.DomainModelLayer.Services;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.InfrastructureLayer.Implementations;
using DDD.FestivalTickets.Core.ApplicationLayer.Mappers;
using FestivalTickets.ConsoleTest;

using Microsoft.Data.Sqlite;

var services = new ServiceCollection();

// 1. Konfiguracja Bazy Danych (SQLite in-memory)
// MUSIMY trzymać jedno połączenie otwarte przez cały czas trwania testu,
// inaczej SQLite in-memory usunie bazę przy zamknięciu połączenia.
var keepAliveConnection = new SqliteConnection("Data Source=:memory:");
keepAliveConnection.Open();

services.AddDbContext<FestivalDbContext>(options =>
    options.UseSqlite(keepAliveConnection));

// 2. Rejestracja Infrastruktury
services.AddScoped<IFestivalUnitOfWork, FestivalUnitOfWork>();
services.AddScoped<IDomainEventPublisher, SimpleEventPublisher>();

// Rejestracja repozytoriów pobieranych z UnitOfWork (dla serwisów które ich potrzebują bezpośrednio)
services.AddScoped<ITicketRepository>(sp => sp.GetRequiredService<IFestivalUnitOfWork>().Tickets);
services.AddScoped<IZoneRepository>(sp => sp.GetRequiredService<IFestivalUnitOfWork>().Zones);
services.AddScoped<ICustomerRepository>(sp => sp.GetRequiredService<IFestivalUnitOfWork>().Customers);

// Dummy Notification Service
services.AddScoped<INotificationService, DDD.FestivalTickets.Core.InfrastructureLayer.NotificationService>();

// 3. Rejestracja Domeny (Fabryki i Serwisy)
services.AddScoped<TicketFactory>();
services.AddScoped<TicketAvailabilityService>();
services.AddScoped<DiscountPolicyFactory>();

// 4. Rejestracja Warstwy Aplikacji (Handlery i Mappery)
services.AddScoped<CommandHandler>();
services.AddScoped<QueryHandler>();
services.AddScoped<Mapper>();

// Event Handlers
services.AddScoped<DDD.SharedKernel.ApplicationLayer.IEventHandler<DDD.FestivalTickets.Core.DomainModelLayer.Events.TicketPurchasedDomainEvent>, DDD.FestivalTickets.Core.ApplicationLayer.DomainEventHandlers.SendConfirmationWhenTicketPurchasedHandler>();

// 5. Rejestracja TestSuite
services.AddScoped<TestSuite>();

var serviceProvider = services.BuildServiceProvider();

// Inicjalizacja bazy danych (stworzenie schematu)
using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FestivalDbContext>();
    context.Database.OpenConnection();
    context.Database.EnsureCreated();
}

// Uruchomienie testów
using (var scope = serviceProvider.CreateScope())
{
    var testSuite = scope.ServiceProvider.GetRequiredService<TestSuite>();
    testSuite.RunTests();
}
