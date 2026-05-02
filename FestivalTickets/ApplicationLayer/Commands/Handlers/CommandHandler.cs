using System;
using System.Linq;
using DDD.FestivalTickets.Core.ApplicationLayer.Commands;
using DDD.FestivalTickets.Core.DomainModelLayer.Factories;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.FestivalTickets.Core.DomainModelLayer.Services;
using DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects;
using DDD.SharedKernel.DomainModelLayer;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Commands.Handlers
{
    public class CommandHandler
    {
        private readonly IFestivalUnitOfWork _unitOfWork;

        private readonly TicketFactory _ticketFactory;
        private readonly TicketAvailabilityService _ticketAvailabilityService;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CommandHandler(
            IFestivalUnitOfWork unitOfWork,
            TicketFactory ticketFactory,
            TicketAvailabilityService ticketAvailabilityService,
            IDomainEventPublisher domainEventPublisher)
        {
            _unitOfWork = unitOfWork;
            _ticketFactory = ticketFactory;
            _ticketAvailabilityService = ticketAvailabilityService;
            _domainEventPublisher = domainEventPublisher;
        }
        
        public void Handle(AddConcertToZoneCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var zone = _unitOfWork.Zones.GetWithConcerts(command.ZoneId);
            if (zone == null)
                throw new InvalidOperationException($"Zone with id {command.ZoneId} was not found.");

            var eventDay = new EventDay(
                command.EventDate,
                command.EventDayName
            );

            var concert = new Concert(
                command.ConcertId,
                command.ArtistName,
                command.Genre,
                command.StartTime,
                eventDay,
                command.ZoneId
            );

            zone.AddConcert(concert);
            _unitOfWork.Commit();
        }

        public void Handle(CancelTicketCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var ticket = _unitOfWork.Tickets.Get(command.TicketId);
            if (ticket == null)
                throw new InvalidOperationException($"Ticket with id {command.TicketId} was not found.");

            ticket.Cancel();

            _unitOfWork.Commit();
        }

        public void Handle(CreateZoneCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var price = new Money(
                command.BaseTicketPrice,
                command.TicketCurrency
            );

            var zone = new Zone(
                command.ZoneId,
                command.ZoneName,
                command.Capacity,
                price
            );

            _unitOfWork.Zones.Insert(zone);
            _unitOfWork.Commit();
        }

        public void Handle(PurchaseTicketCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var zone = _unitOfWork.Zones.Get(command.ZoneId);
            if (zone == null)
                throw new InvalidOperationException($"Zone with id {command.ZoneId} was not found.");

            var customer = _unitOfWork.Customers.Get(command.CustomerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer with id {command.CustomerId} was not found.");

            var eventDay = new EventDay(
                command.EventDate,
                command.EventDayName
            );

            _ticketAvailabilityService.CheckAvailability(zone, customer, eventDay);

            var ticket = _ticketFactory.Create(
                command.TicketId,
                zone,
                customer,
                eventDay
            );

            _unitOfWork.Tickets.Insert(ticket);
            _unitOfWork.Commit();
            PublishDomainEvents(ticket);
        }
        
        public void Handle(RegisterCustomerCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var existingCustomer = _unitOfWork.Customers.GetByEmail(command.Email);
            if (existingCustomer != null)
                throw new InvalidOperationException($"Customer with email {command.Email} already exists.");

            var customerType = new CustomerType(command.CustomerTypeValue);

            var customer = new Customer(
                command.CustomerId,
                command.FirstName,
                command.LastName,
                command.Email,
                customerType
            );
            _unitOfWork.Customers.Insert(customer);
            _unitOfWork.Commit();
            PublishDomainEvents(customer);
        }

        private void PublishDomainEvents(Entity entity)
        {
            foreach (var domainEvent in entity.DomainEvents.ToList())
            {
                _domainEventPublisher.Publish((dynamic)domainEvent);
            }

            entity.RemoveAllDomainEvents();
        }
    }
}
