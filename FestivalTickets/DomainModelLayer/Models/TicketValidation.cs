using System;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Models
{
    /// <summary>
    /// ENCJA wewnątrz agregatu Ticket.
    /// Rejestruje fakt zeskanowania biletu przy bramce wejściowej.
    ///
    /// Istnienie tej encji rozwiązuje problem ponownego wejścia:
    /// jeśli _validations.Count > 0, bilet był już użyty.
    ///
    /// Tabela: TicketValidations
    ///   Id          (PK)
    ///   TicketId    (FK do Tickets)
    ///   GateName    – nazwa bramki, np. "Brama Główna", "Wejście VIP"
    ///   ScannedAt   – kiedy zeskanowano
    ///
    /// TicketValidation NIE jest agregatem.
    /// Dostęp tylko przez Ticket.MarkAsUsed() – nigdy bezpośrednio.
    /// </summary>
    public class TicketValidation : Entity
    {
        public long     TicketId  { get; protected set; }  // FK – nie referencja do Ticket!
        public string   GateName  { get; protected set; }
        public DateTime ScannedAt { get; protected set; }

        // wymagany przez EF Core
        protected TicketValidation() { }

        public TicketValidation(long id, long ticketId, string gateName, DateTime scannedAt)
            : base(id)
        {
            if (string.IsNullOrWhiteSpace(gateName))
                throw new ArgumentException("Nazwa bramki nie może być pusta.");

            TicketId  = ticketId;
            GateName  = gateName.Trim();
            ScannedAt = scannedAt;
        }

        public override string ToString()
            => $"Bramka: {GateName} | Zeskanowano: {ScannedAt:dd.MM.yyyy HH:mm:ss}";
    }
}
