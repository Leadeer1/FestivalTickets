using System;
using System.Collections.Generic;
using System.Linq;
using DDD.FestivalTickets.Core.ApplicationLayer.DTOs;
using DDD.FestivalTickets.Core.ApplicationLayer.Mappers;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Queries.Handlers
{
    //zwraca EF dla prostych, nie wymagających logiki biznesowej zapytań, które nie mają dedykowanego handlera
    //zwraca Dapper dla Joinów
    public class QueryHandler
    {
        private readonly IFestivalUnitOfWork _unitOfWork;
        private Mapper _mapper;

        public QueryHandler(IFestivalUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CustomerDTO Handle(GetCustomerQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var customer = _unitOfWork.Customers.Get(query.CustomerId);
            if (customer == null)
                throw new InvalidOperationException($"Customer with id {query.CustomerId} was not found.");

            return this._mapper.Map(customer);
        }

        public IList<CustomerDTO> Handle(GetAllCustomersQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return _unitOfWork.Customers.GetAll().Select(_mapper.Map).ToList();
        }

        public TicketDTO Handle(GetTicketQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var ticket = _unitOfWork.Tickets.Get(query.TicketId);
            if (ticket == null)
                throw new InvalidOperationException($"Ticket with id {query.TicketId} was not found.");

            return this._mapper.Map(ticket);
        }

        public IList<TicketDTO> Handle(GetAllTicketsQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return _unitOfWork.Tickets.GetAll().Select(_mapper.Map).ToList();
        }

        public ZoneDTO Handle(GetZoneQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var zone = _unitOfWork.Zones.GetWithConcerts(query.ZoneId);
            if (zone == null)
                throw new InvalidOperationException($"Zone with id {query.ZoneId} was not found.");

            return this._mapper.Map(zone);
        }

        public IList<ZoneDTO> Handle(GetAllZonesQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return _unitOfWork.Zones.GetAllWithConcerts().Select(_mapper.Map).ToList();
        }

    }
}
