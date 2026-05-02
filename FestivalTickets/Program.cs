using DDD.FestivalTickets.Core.DomainModelLayer.Factories;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.FestivalTickets.Core.InfrastructureLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        // InMemory do testów – zamień na AddInfrastructure(ctx.Configuration) gdy masz SQL Server
        services.AddInfrastructureInMemory();
        services.AddLogging(b => b.AddConsole().SetMinimumLevel(LogLevel.Warning));
    })
    .Build();

Console.WriteLine("=== Test Infrastructure Layer ===\n");

using var scope = host.Services.CreateScope();
var uow = scope.ServiceProvider.GetRequiredService<IFestivalUnitOfWork>();

// 1. Dodaj strefę
var zone = new Zone(
    id: 1,
    name: "Scena Główna",
    capacity: 500,
    baseTicketPrice: new Money(199.99m, "PLN")
);
uow.Zones.Insert(zone);
uow.Commit();
Console.WriteLine($"✅ Strefa: {zone.Name} | Cena: {zone.BaseTicketPrice.Amount} PLN");

// 2. Dodaj klienta
var customer = new Customer(
    id: 1,
    firstName: "Jan",
    lastName: "Kowalski",
    email: "jan@example.com",
    customerType: new CustomerType(CustomerTypeValue.Student)
);
uow.Customers.Insert(customer);
uow.Commit();
Console.WriteLine($"✅ Klient: {customer.FullName} | Typ: {customer.CustomerType}");

// 3. Utwórz bilet przez TicketFactory
var eventDay = new EventDay(new DateTime(2025, 8, 15), "Dzień 1");
var factory  = new TicketFactory(new DiscountPolicyFactory());
var ticket   = factory.Create(ticketId: 1, zone: zone, customer: customer, eventDay: eventDay);
uow.Tickets.Insert(ticket);
uow.Commit();
Console.WriteLine($"✅ Bilet: {ticket.Id} | Cena finalna: {ticket.FinalPrice.Amount} PLN (zniżka 50% dla studenta)");

// 4. Odczyt
var readZone     = uow.Zones.GetWithConcerts(zone.Id);
var readCustomer = uow.Customers.GetByEmail("jan@example.com");
var ticketCount  = uow.Tickets.CountActiveByZoneAndDay(zone.Id, eventDay);

Console.WriteLine($"\n📋 Strefa z bazy:    {readZone?.Name}");
Console.WriteLine($"📋 Klient z bazy:    {readCustomer?.FullName}");
Console.WriteLine($"📋 Biletów w strefie: {ticketCount}");

Console.WriteLine("\n✅ Infrastructure Layer działa poprawnie!");
