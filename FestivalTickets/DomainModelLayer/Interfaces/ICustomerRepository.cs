using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using DDD.SharedKernel.InfrastructureLayer;

namespace DDD.FestivalTickets.Core.DomainModelLayer.Interfaces
{
    /// <summary>
    /// Repozytorium agregatu Customer.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Wyszukuje klienta po adresie email.
        /// Używane przy rejestracji – sprawdzenie czy email już istnieje.
        /// Zwraca null jeśli nie znaleziono.
        /// </summary>
        Customer GetByEmail(string email);
    }
}
