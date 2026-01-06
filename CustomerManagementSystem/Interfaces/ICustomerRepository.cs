using System.Collections.Generic;
using CustomerSystem.Entities;

namespace CustomerSystem.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> SearchByCountry(string country);
        List<Customer> SearchByCompanyName(string company);
        List<Customer> SearchByContact(string contact);
    }
}