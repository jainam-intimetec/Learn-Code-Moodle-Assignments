using System.Collections.Generic;
using System.Text;
using CustomerSystem.Entities;

namespace CustomerSystem.Services
{
    public class CsvExporter:ICustomerExporter
    {
        public string Export(List<Customer> customerData)
        {
          StringBuilder sb = new StringBuilder();

           foreach(var customer in customerData)
            {
                sb.AppendFormat("{0},{1}, {2}, {3}", customer.CustomerID, customer.CompanyName, customer.ContactName, customer.Country);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}