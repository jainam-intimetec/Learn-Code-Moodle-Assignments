using System.Collections.Generic;
using CustomerSystem.Entities;

namespace CustomerSystem.Interfaces
{
    public interface ICustomerExporter
    {
        string Export(List<Customer> customerData);
    }
}