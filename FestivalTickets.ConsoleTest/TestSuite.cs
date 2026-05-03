using System;
using System.Linq;
using DDD.FestivalTickets.Core.ApplicationLayer.Commands;
using DDD.FestivalTickets.Core.ApplicationLayer.Commands.Handlers;
using DDD.FestivalTickets.Core.ApplicationLayer.Queries;
using DDD.FestivalTickets.Core.ApplicationLayer.Queries.Handlers;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;

namespace FestivalTickets.ConsoleTest
{
    public class TestSuite
    {
        private readonly CommandHandler _commandHandler;
        private readonly QueryHandler _queryHandler;

        public TestSuite(CommandHandler commandHandler, QueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        public void RunTests()
        {
            Console.WriteLine("--- ROZPOCZĘCIE SCENARIUSZA E2E ---");

            // 1. Rejestracja Klienta
            var customerId = 1L;
            _commandHandler.Handle(new RegisterCustomerCommand
            {
                CustomerId = customerId,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@krakow.pl",
                CustomerTypeValue = CustomerTypeValue.KrakowResident
            });
            Console.WriteLine("[OK] Zarejestrowano klienta.");

            // 2. Tworzenie Strefy
            var zoneId = 100L;
            _commandHandler.Handle(new CreateZoneCommand
            {
                ZoneId = zoneId,
                ZoneName = "Golden Circle",
                Capacity = 2,
                BaseTicketPrice = 250m,
                TicketCurrency = "PLN"
            });
            Console.WriteLine("[OK] Utworzono strefę Golden Circle.");

            // 3. Dodanie koncertu do strefy
            _commandHandler.Handle(new AddConcertToZoneCommand
            {
                ZoneId = zoneId,
                ConcertId = 500L,
                ArtistName = "Iron Maiden",
                Genre = "Heavy Metal",
                StartTime = DateTime.Now.AddHours(20),
                EventDate = DateTime.Today,
                EventDayName = "Dzień 1"
            });
            Console.WriteLine("[OK] Dodano koncert do strefy.");

            // 4. Zakup biletu
            var ticketId = 1000L;
            _commandHandler.Handle(new PurchaseTicketCommand
            {
                TicketId = ticketId,
                CustomerId = customerId,
                ZoneId = zoneId,
                EventDate = DateTime.Today,
                EventDayName = "Dzień 1"
            });
            Console.WriteLine("[OK] Zakupiono bilet.");

            // 5. Weryfikacja bazy przez QueryHandler
            var ticket = _queryHandler.Handle(new GetTicketQuery { TicketId = ticketId });
            var customer = _queryHandler.Handle(new GetCustomerQuery { CustomerId = customerId });

            Console.WriteLine($"--- WYNIK TESTU ---");
            Console.WriteLine($"Bilet ID: {ticket.Id}");
            Console.WriteLine($"Cena Ostateczna: {ticket.FinalPriceAmount} {ticket.FinalPriceCurrency}");
            Console.WriteLine($"Klient: {customer.FirstName} {customer.LastName}");
            
            Console.WriteLine("--- KONIEC SCENARIUSZA E2E ---");
        }
    }
}
