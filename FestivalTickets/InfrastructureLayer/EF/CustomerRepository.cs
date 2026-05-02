using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDD.FestivalTickets.Core.DomainModelLayer.Interfaces;
using DDD.FestivalTickets.Core.DomainModelLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DDD.FestivalTickets.Core.InfrastructureLayer.EF
{
    internal class CustomerRepository : ICustomerRepository
    {
        private readonly FestivalDbContext _db;

        public CustomerRepository(FestivalDbContext db)
        {
            _db = db;
        }

        public Customer Get(long id)
            => _db.Customers
                  .Include(c => c.Email)       // OwnsOne – EF ładuje automatycznie
                  .Include(c => c.CustomerType) // OwnsOne – EF ładuje automatycznie
                  .FirstOrDefault(c => c.Id == id);

        public IList<Customer> GetAll()
            => _db.Customers.ToList();

        public IList<Customer> Find(Expression<Func<Customer, bool>> expression)
            => _db.Customers.Where(expression).ToList();

        public void Insert(Customer entity)
            => _db.Customers.Add(entity);

        public void Delete(Customer entity)
            => _db.Customers.Remove(entity);

        public Customer GetByEmail(string email)
            => _db.Customers
                  .FirstOrDefault(c => c.Email.Value == email);
    }
}
