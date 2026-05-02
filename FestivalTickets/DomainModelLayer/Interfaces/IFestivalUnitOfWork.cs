using DDD.SharedKernel.InfrastructureLayer;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Unit of Work dla systemu FestivalTickets.
    /// Odpowiednik IEscapeRoomUnitOfWork z projektu EscapeRoom.
    ///
    /// Agreguje wszystkie repozytoria – CommandHandler wstrzykuje tylko ten jeden interfejs
    /// i ma dostęp do całej warstwy persystencji + atomowego Commit().
    ///
    /// Użycie w CommandHandler:
    ///   _uow.Zones.Insert(zone);
    ///   _uow.Tickets.Insert(ticket);
    ///   _uow.Commit();
    /// </summary>
    public interface IFestivalUnitOfWork : IUnitOfWork
    {
        IZoneRepository     Zones     { get; }
        ITicketRepository   Tickets   { get; }
        ICustomerRepository Customers { get; }
    }
}
