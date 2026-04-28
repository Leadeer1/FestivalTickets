using System.Collections.Generic;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.SharedKernel.InfrastructureLayer;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Repozytorium agregatu Zone.
    /// Metody bazowe (Get, GetAll, Find, Insert, Delete) dziedziczone z IRepository.
    /// Metody specyficzne dla domeny zdefiniowane poniżej.
    /// </summary>
    public interface IZoneRepository : IRepository<Zone>
    {
        /// <summary>
        /// Pobiera Zone wraz z załadowaną kolekcją Concerts (eager loading).
        /// Używane gdy potrzebujemy dodać koncert lub sprawdzić program strefy.
        /// </summary>
        Zone GetWithConcerts(long zoneId);

        /// <summary>
        /// Pobiera wszystkie Zone wraz z ich koncertami.
        /// Używane w QueryHandler dla GetAllZonesQuery.
        /// </summary>
        IList<Zone> GetAllWithConcerts();
    }
}
