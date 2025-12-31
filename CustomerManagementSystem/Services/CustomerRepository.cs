using System.Collections.Generic;
using System.Linq;
using CustomerSystem.Entities;
using CustomerSystem.Interfaces;

namespace CustomerSystem.Services
{
    public class CustomerRepository : ICustomerRepository
    {
       
        private IQueryable<Customer> GetBaseQuery() => 
            db.customers.OrderBy(c => c.CustomerID);

        public List<Customer> SearchByCountry(string country) =>
            GetBaseQuery().Where(c => c.Country.Contains(country)).ToList();

        public List<Customer> SearchByCompanyName(string company) =>
            GetBaseQuery().Where(c => c.CompanyName.Contains(company)).ToList();

        public List<Customer> SearchByContact(string contact) =>
            GetBaseQuery().Where(c => c.ContactName.Contains(contact)).ToList();
    }
}